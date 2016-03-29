// define するシンボル（プロジェクトのプロパティで設定）
//  ON: WINDOWS,CALC_FPS,MOUSE_UNDEAD,NO_ROTATION
// OFF: JAPANESE_MAP,LABEL_JAPANESE,NO_DRAW,NoEyeTrack,STRICT,STROKE_DEBUG

#region Using Statements
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
//using System.Threading;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using PhotoViewer.InputDevice;
using PhotoViewer.Manager;
using PhotoViewer.Element;
using PhotoViewer.Supplement;
//using Eye_Tracker_Component;
//using FlickrNet;
using System.Diagnostics;
using PhotoViewer.Controller;
//using System.Windows.Forms;

using Windows7.Multitouch.Window;
using Windows7.Multitouch;
#endregion

namespace PhotoViewer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Browser : Microsoft.Xna.Framework.Game
    {
        #region 基本設定
        // タイトルテキスト
        public static string Title = "D-Flip: Dynamic & Flexible Interactive Photoshow";
        public static Browser Instance;
        // （モニタの解像度が 1024×768 のとき，(1024, 768) - (6, 32) → (1018, 736) とするとウインドウが画面いっぱいになる）
        // ムービー撮影時は，ウインドウ枠を除く領域の 横:縦 を 64:48 の倍数にする（アスペクト比 4:3，圧縮時のノイズを減らすため 16 の倍数）．
        public int ClientWidth 
        {
            get;
            private set;
        }// 1274;// 1018;
        public int ClientHeight 
        {
            get;
            private set;
        }// 992;// 736;
        // 合适的图像区域
        public const int MAXX = 128;
        public const int MAXY = 128;
        // 白色边框的厚度
        public const int MAR = 10;//5

        public System.Windows.Forms.Control control
        {
            get;
            private set;
        }

        
        KeyboardDevice keyboard = new KeyboardDevice();

        // 每一幅图片的最大最小缩放值
        public static float MinPhotoScale(int windowX, int windowY, int photoX, int photoY, int photoCount)
        {
            int wArea = windowX * windowY;
            int pArea = photoX * photoY;
            int pCount = (1 + (int)Math.Sqrt((double)photoCount)) * (1 + (int)Math.Sqrt((double)photoCount));
            return (float)Math.Sqrt((float)wArea / (float)(pCount * pArea)) * 0.25f;
        }
        public static float MaxPhotoScale(int windowX, int windowY, int photoX, int photoY, int photoCount)
        {
            int wArea = windowX * windowY;
            int pArea = photoX * photoY;
            return (float)Math.Sqrt((float)wArea / (float)pArea) * 0.5f /*0.75f*/;
        }
        
        // 自动载入图像周期[ms]
        public const int AutoLoadSpan = 5000;
        // 采样过滤器周期[ms]
        public const int FilterSamplingTime = 10;//1000;
        // 入力がないポインティングデバイスのポインタを削除するまでの時間[frame]
#if MOUSE_UNDEAD
        private readonly int RAW_MOUSE_REMOVE = int.MaxValue;// 30 * 60
#else
        private readonly int RAW_MOUSE_REMOVE = 30 * 60;
#endif
        #endregion

        #region 成员
        // 一致随机数生成
        //private Random random_ = new Random();
        // 高斯分布随机生成（Box Muller法）
        //private RandomBoxMuller randombm_ = new RandomBoxMuller();
        // 写真列表
        //private List<Photo> photos_ = new List<Photo>();
        // フレーム数のカウンタ
        //private double checkTimeForFiltering_ = 0;
        // 是否优化
        //private bool isOptimize_ = true;
        // 各种窗体
        private SystemState systemState;
        private PhotoDisplay photoDisplay;// new Viewer();
#if NoEyeTrack
#else
        private EyeTrackingForm eyeTrackingForm_ = new EyeTrackingForm();
#endif
        #endregion

        #region 描画用成员和纹理
        // 描画用グラフィックデバイスマネージャ
        GraphicsDeviceManager graphics_;
        // 描画用スプライトバッチ
        public SpriteBatch batch_
        {
            get;
            private set;
        }
        #endregion

        #region ControlPanel のメンバに置き換える等して消す予定
        // アトラクター選択用のアイコン（ドック風）
        private Dock dock_ = null;
        
        // 存储stroke的列表
        
        private StrokeBoxCollection strokeGroup; 
        // pie menu 列表
        private List<PieMenu> pieMenus_ = new List<PieMenu>();
        
        //Time state resource
        private TimeSliderManager timeStateManager = new TimeSliderManager();
        #endregion

        // 加载照片的日志
        //private List<PhotoLog> photoLog_ = new List<PhotoLog>();
        // 加载人物标记, added by Gengdai
        //private List<PeopleTags> peopleTags = new List<PeopleTags>();
        

        MouseController inputController;
        RawInputForm rawInput;
        PointingDeviceCollection pdCollection;
        
        // 构造函数
        public Browser()
        {
            // 图形和内容管理器实例化
            graphics_ = new GraphicsDeviceManager(this);
            
            // 抗锯齿有效
            graphics_.PreferMultiSampling = true;
            
            // 设置窗口大小
            ClientHeight = 1080-32;
            ClientWidth = 1920-6;
            graphics_.PreferredBackBufferWidth = ClientWidth;
            graphics_.PreferredBackBufferHeight = ClientHeight;
            // 设置窗口名称
            this.Window.Title = Title;
            // 可以调整窗口大小
            this.Window.AllowUserResizing = true;
            //graphics_.IsFullScreen = true;
            // 窗口不显示鼠标
            this.IsMouseVisible = false;
            // 是否固定60 FPS（詳細はXNAの仕様を参照 http://blogs.msdn.com/ito/archive/2007/03/08/2-update.aspx）
#if CALC_FPS
            this.IsFixedTimeStep = false;
#else
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
#endif
            graphics_.ApplyChanges();
            // 读取设定文件（图像日志）
            //controlPanel_.ReadPhotoLogs(photoLog_);
            //controlPanel_.ReadPeopleLogs(peopleTags);
            IntPtr hWnd = this.Window.Handle;
            control = System.Windows.Forms.Control.FromHandle(hWnd);
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            
            //batch_ = new SpriteBatch(this.GraphicsDevice);
            //provide global access to game instance, so other classes can access graphicsdevice
            Instance = this;
            
            clientBounds = new BoundingBox2D(new Vector2(Window.ClientBounds.Left, Window.ClientBounds.Top), new Vector2(Window.ClientBounds.Right, Window.ClientBounds.Bottom), 0f);
            rawInput = new RawInputForm();
            pdCollection = rawInput.pdCollection;
            //one more pointing device, store touch data
            //pdCollection.add(new PointingDevice(0, new Vector2(6000, 6000)));
            systemState = new SystemState();
            strokeGroup = new StrokeBoxCollection();
            box.ShowTextBox(Vector2.Zero, strokeGroup.strokeList[0]);

        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ClientHeight = this.Window.ClientBounds.Height;
            ClientWidth = this.Window.ClientBounds.Width;
            box.showAgain(Vector2.Zero);
        }

        // 丢弃
        protected override void Dispose(bool disposing)
        {
            // 丢弃，释放资源
            if (disposing)
            {
                rawInput.Dispose();
                rawInput = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            dock_ = new Dock(ResourceManager.iconNumber_);
            //pdCollection.initialize();
            photoDisplay = new PhotoDisplay(systemState, dock_, timeStateManager.sBar, strokeGroup);
            inputController = new MouseController(pdCollection, dock_, timeStateManager.sBar, systemState, photoDisplay, strokeGroup, keyboard);
            
        }
        /*public Stream GenerateStreamFromString(string s)
{
    MemoryStream stream = new MemoryStream();
    StreamWriter writer = new StreamWriter(stream);
    writer.Write(s);
    writer.Flush();
    stream.Position = 0;
    return stream;
}*/


        /// <summary>
        /// Load your graphics_ content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        [Obsolete("LoadGraphicsContentは古い形式")]
        protected override void LoadContent()
        {
            batch_ = new SpriteBatch(graphics_.GraphicsDevice);
            ResourceManager.LoadContent();
        }
        List<PointingDevice> touchDevices = new List<PointingDevice>();
        private List<FloatTextBox> ftBoxes_ = new List<FloatTextBox>();
        
        
        /// <summary>
        /// Unload your graphics_ content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        [Obsolete("UnloadGraphicsContentは古い形式")]
        protected override void UnloadContent()
        {
            if (true)
            {
                // TODO: Unload any ResourceManagementMode.Automatic content
                batch_.Dispose(); // NOTE: 也不知道是否是必要的

                //if (createPhoto != null)
                //{
                //    createPhoto.UnloadPhoto();
                //    createPhoto.UnloadPeopleTag();
                //}
                //peopleTags.Clear();
                ResourceManager.Unload();
                //content_.Unload();
            }

            // TODO: Unload any ResourceManagementMode.Manual content
        }
        //Dictionary<Photo, int> touchPhotoMap = new Dictionary<Photo,int>(); 
        public BoundingBox2D clientBounds = new BoundingBox2D();
        //Dictionary<int, Photo> touchMap = new Dictionary<int, Photo>();
        private void addNewPointingDevice(List<PointingDevice> deviceList, PointingDevice newDevice)
        {
            deviceList.Add(newDevice);
            int count = deviceList.Count();
            deviceList[count-1].PositionAdd(-clientBounds.Min);
            deviceList[count - 1].RightDownPosition -= clientBounds.Min;
            deviceList[count - 1].LeftDownPosition -= clientBounds.Min;
            deviceList[count - 1].MiddleDownPosition -= clientBounds.Min;
        }
        FloatTextBox box = new FloatTextBox();

        //List<PointingDevice> touchPoint;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
#if CALC_FPS
            experimentForm_.LogUpdateTime(gameTime, photos_.Count);
#endif
            
#if NoEyeTrack
#else
            eyeTrackingForm_.ScreenBounds = screenBounds;
#endif

            clientBounds = new BoundingBox2D(new Vector2(Window.ClientBounds.Left, Window.ClientBounds.Top), new Vector2(Window.ClientBounds.Right, Window.ClientBounds.Bottom), 0f);
                        
            #region 指点设备状态更新
            
            touchDevices.Clear();

             inputController.trigerDock();
                

            // アイトラッカをポインティングデバイスとして追加
#if NoEyeTrack
#else
            if (eyeTrackingForm_.IsTracking)
            {
                // 両目の視点の平均を利用   利用视点的平均值
                pointingDevices.Add(new PointingDevice(-1, PointingDevice.DeviceType.EyeTracker, eyeTrackingForm_.GazePosition - clientBounds.Min));
                draggedPhotos_.Add(new List<SDraggedPhoto>());
                //// 左右の視点を別々に利用
                //pointingDevices.Add(new PointingDevice(-2, PointingDevice.DeviceType.EyeTracker, eyeTrackingForm_.LeftGazePosition - clientBounds.Min));
                //pointingDevices.Add(new PointingDevice(-3, PointingDevice.DeviceType.EyeTracker, eyeTrackingForm_.RightGazePosition - clientBounds.Min));
            }
#endif

            #endregion

            
#region new picture color calculation
            //        if (lastPhoto.Feature == null || lastPhoto.Feature.Length != Photo.FeatureSplit * Photo.FeatureSplit)
            //        {
            //            lastPhoto.CalcFeature();
            //            // 已经取得的特征
            //            List<string> f = new List<string>();
            //            for (int i = 0, len = lastPhoto.Feature.Length; i < len; ++i)
            //            {
            //                f.Add(lastPhoto.Feature[i].X.ToString());
            //                f.Add(lastPhoto.Feature[i].Y.ToString());
            //                f.Add(lastPhoto.Feature[i].Z.ToString());
            //            }
            //            nowLog.Feature = f.ToArray();
            //            nowLog.Variance = lastPhoto.Variance.ToString();
            //            controlPanel_.SavePhotoLogs(photoLog_);
            //        }
            //        if (nlIndex < 0)
            //        {
            //            photoLog_.Add(nowLog);
            //        }
            //        lastPhoto.Center = new Vector2(t.Width, t.Height) * 0.5f;
            //        lastPhoto.BoudingBox = new BoundingBox2D(lastPhoto.Position - lastPhoto.Center * lastPhoto.Scale - Vector2.One * (float)Browser.MAR, lastPhoto.Position + lastPhoto.Center * lastPhoto.Scale + Vector2.One * (float)Browser.MAR, lastPhoto.Angle);
            //        if (controlPanel_.FileNames.Count == 0)
            //        {
            //            photoLog_.Sort(); //排序
            //            controlPanel_.SavePhotoLogs(photoLog_); //保存
            //        }
            //    }
            //}
#endregion

            ///
            /// 用户的操作 
            ///
            #region  keyboard
            keyboard.getKeyboardState();
            //isCtrl = keyboard.ctrlKey;
            #endregion
            //box.showAgain(Vector2.Zero);

            //for (int i = 0; i < pdCount; ++i)
            //{
            //    #region 鼠标右键的处理
            //    PointingDevice p2 = input_.PointingDevices[i];
            //    PointingDevice p1 = input_.PointingDevicesBefore[i];
            //    if (p2.RightButton == ButtonState.Released)
            //    {
            //        if (pieMenus_[i].Mode == PieMenu.PieMode.DeletePhoto) // 删除图片
            //        {
            //            // 删除图像
            //            if (!pdIntercepted[i])
            //            {
            //                if (( p2).LeftButton == ButtonState.Pressed)
            //                {
            //                    foreach (Photo a in photos_)
            //                    {
            //                        if (a.IsOverlapsMouses[i])
            //                        {
            //                            a.IsDel = true;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //strokeGroup.remove();


            ////  清除不出现的图片的标签
            //foreach (Photo a in photos_)
            //{
            //    if (a.IsDel)
            //    {
            //        if (a.ScaleDisplay < 0)
            //        {
            //            for (int i = 0; i < pdCount; ++i)
            //            {
            //                int deleteIndex = -1;
            //                for (int j = 0, jlen = draggedPhotos_[i].Count; j < jlen; ++j)
            //                {
            //                    if (draggedPhotos_[i][j].ID == a.ID)
            //                    {
            //                        deleteIndex = j;
            //                        break;
            //                    }
            //                }
            //                if (deleteIndex != -1)
            //                {
            //                    draggedPhotos_[i].RemoveAt(deleteIndex);
            //                }
            //            }
            //            photos_.Remove(a);
            //            break;
            //        }
            //    }
            //}
            
            photoDisplay.photoInitialize();
            inputController.check();
            photoDisplay.photoBehavior();

            #region 前処理
            

            #region touch drag
            //for (int i = 0; i < tcCount; i++)
            //{
            //    if (!touchMap.ContainsKey(touchDevices[i].Header))
            //    {
            //        foreach(Photo p in photos_)
            //        {
            //            if(p.BoundingBoxDisplay.Contains(touchDevices[i].Position) == ContainmentType.Contains)
            //            {
            //                p.IsClicked = true;
            //                p.ClickedPoint = p.BoundingBoxDisplay.Offset(touchDevices[i].Position);
            //                bool flag = false;
            //                for (int j = 0; j < draggedPhotos_[pdCount - 1].Count; j++)
            //                {
            //                    if (draggedPhotos_[pdCount - 1][j].ID == p.ID)
            //                    {
            //                            var something = draggedPhotos_[pdCount - 1][j];
            //                            something.DraggedPosition = (p.Position - touchDevices[i].Position) / p.ScaleDisplay;
            //                            draggedPhotos_[pdCount - 1][j] = something;
            //                            //Debug.WriteLine(draggedPhotos_[pdCount-1][j].DraggedPosition);
            //                            p.Position = touchDevices[i].Position + draggedPhotos_[pdCount - 1][j].DraggedPosition * p.ScaleDisplay;
            //                            p.PositionDisplay = p.Position;
            //                            flag = true;
            //                            break;
            //                    }
            //                }
            //                if (!flag)
            //                {
            //                    // ドラッグ時のタグを 一時的に登録
            //                    List<string> dragPhotoTags = new List<string>();
            //                    SDraggedPhoto sd = new SDraggedPhoto(p.ID, (p.PositionDisplay - touchDevices[i].Position) / p.ScaleDisplay, dragPhotoTags);
            //                    draggedPhotos_[pdCount - 1].Add(sd);
            //                    p.Position = touchDevices[i].Position + sd.DraggedPosition * p.ScaleDisplay;
            //                    p.PositionDisplay = p.Position;
            //                }
                            
            //                touchPhotoMap[p] = 0;
            //                touchMap[touchDevices[i].Header] = p;
            //                break;
            //            }
            //        }
            //    }
            //    else 
            //    {
            //        Photo p = touchMap[touchDevices[i].Header];
            //        if (p.BoudingBoxDisplay.Contains(touchDevices[i].Position) == ContainmentType.Contains)
            //        for (int j = 0; j < draggedPhotos_[pdCount - 1].Count; j++)
            //        {
            //            if (draggedPhotos_[pdCount - 1][j].ID == p.ID)
            //            {
                            
            //                p.Position = touchDevices[i].Position + draggedPhotos_[pdCount - 1][j].DraggedPosition * p.ScaleDisplay;
            //                p.PositionDisplay = p.Position;
            //                //flag = true;
            //                touchPhotoMap[p] = 0;
            //                break;
            //            }

            //        }
            //    }
            //}
            //var keys = new List<Photo>(touchPhotoMap.Keys);
            //foreach (var key in keys)
            //{
            //    touchPhotoMap[key]++;
            //    if (touchPhotoMap[key] < 200)
            //    {
            //        key.KeepGazed(pdCount - 1);
            //        key.LayerDepth = (float)touchPhotoMap[key] * 0.005f;
            //        //List<string> dragPhotoTags = new List<string>();
            //        //draggedPhotos_[pdCount-1].Add(new SDraggedPhoto(key.ID, (key.PositionDisplay) / key.ScaleDisplay, dragPhotoTags));
            //    }
            //    else//if time >= 200, turn to not-selected
            //    {
            //        touchPhotoMap.Remove(key);
            //        key.LayerDepth = 1f;
            //        foreach (SDraggedPhoto p in draggedPhotos_[pdCount - 1])
            //        {
            //            if (p.ID == key.ID)
            //            {
            //                draggedPhotos_[pdCount - 1].Remove(p);
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion 

            #endregion
                        
            //foreach (var tl in rawInputForm_.TouchRemoveList)
            //{
            //    touchMap.Remove(tl);
            //}
            rawInput.removeTouchMap();//remove not-existing touch point; if remove in touchup function, can't detect tap.

            base.Update(gameTime);
        }

        Vector3 tempColor;
        bool isBegin = false;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics_.GraphicsDevice.Clear(Color.White);
            if (!isBegin)
            {
                batch_.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                isBegin = true;
            }

            // 描绘地图
            //double mot = -1d;
            //if (systemState.curState == SystemState.ATTRACTOR_GEOGRAPH)
            //{
            //    Color halfWhite = new Color(255, 255, 255, (byte)122);
            //    Vector2 mapScale = new Vector2((float)Window.ClientBounds.Width / (float)ResourceManager.mapTex_.Width, (float)Window.ClientBounds.Height / (float)ResourceManager.mapTex_.Height);
            //    //batch_.Begin();
            //    batch_.Draw(ResourceManager.mapTex_, Vector2.Zero, null, halfWhite, 0f, Vector2.Zero, mapScale, SpriteEffects.None, 1f);
            //    //batch_.End();

            //}
            //// render stroke
            strokeGroup.renderStatic();


            //batch_.End();

            // 渲染画像
            //batch_.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            try
            {
                photoDisplay.drawPhoto();
                //batch_.End();

                //batch_.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                // 渲染dock
                dock_.Render(batch_, Window.ClientBounds.Height, ResourceManager.shadowCircle_, ResourceManager.icon_light_);
                //pdCollection.drawPieMenu();
                //strokeGroup.renderDynamic();
                //strokeGroup.renderTags();
                //渲染滚动条下的dock
                //if (systemState.curState == SystemState.ATTRACTOR_TIME)
                //{
                //    timeStateManager.render();
                //}
                //batch_.End();


                // 渲染光标
                if (!IsMouseVisible)
                {


                    //float alpha = (float)Math.Abs((gameTime.TotalGameTime.Milliseconds % 511) - 255) / 255f;
                    //batch_.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    // 计算鼠标的颜色

                    float hueSpeed = 5f;// / ((float)(this.TargetElapsedTime.Milliseconds) * 0.03f);
                    Vector3 cursorColor = Vector3.Zero;
                    float hu = (float)(gameTime.TotalGameTime.TotalMilliseconds) * hueSpeed / 10000f;
                    if (hu > 1f)
                        hu -= (int)hu;
                    tempColor = new Vector3(hu, 1f, 1f);
                    ResourceManager.hsv2rgb(ref tempColor, out cursorColor);
                    pdCollection.drawMouse(new Color(cursorColor));


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
            }
            finally
            {
                if (isBegin)
                {
                    isBegin = false;
                    batch_.End();
                }
                base.Draw(gameTime);
            }
        }
            
    }
}

