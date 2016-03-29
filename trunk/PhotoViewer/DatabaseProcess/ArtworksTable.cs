using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.Xna.Framework;
using PhotoViewer;
using PhotoInfo;
using System.IO;

namespace database
{
    class ArtworksTable: TableProcessor
    {
        DBConnect db = new DBConnect();
        public Dictionary<string, PhotoTag> select(List<string> fileName)
        {
            Dictionary<string, PhotoTag> fileTags = new Dictionary<string, PhotoTag>();
            //return fileTags;

            //StreamReader reader = new StreamReader("data.xml");
            //var d = reader.ReadToEnd();
            //foreach (var a in ArtworksTag.FromXml(d))
            //{
            //    fileTags[a.allTags[0]] = a;
            //}


            ///////////////////////create table
            //string query = "create table colorInfo ( fileName varchar(45) NOT NULL PRIMARY KEY, f1 float";
            //    for(int i = 2; i <= 75; i++)
            //    {
            //        query += ", ";
            //        query += "f" + i.ToString() + " float";
            //    }
            //query += ", variance float );";
            //if (db.OpenConnection() == true)
            //{
            //    //Create Command
            //    MySqlCommand cmd = new MySqlCommand(query, db.connection);
            //    //Create a data reader and Execute the command
            //    MySqlDataReader dataReader = cmd.ExecuteReader();
            //}
            ///////////////////////////

            //return fileTags;

            //if (fileName.Count == 0)
            //    return fileTags;

            string query = "SELECT * FROM artwork WHERE fileName = '" + fileName[0] + "'";
            for (int i = 1; i < fileName.Count; i++)
            {
                query += " or fileName = '" + fileName[i] + "'";
            }
            query = query.Replace(@"\", @"\\");
            //Open connection
            if (db.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, db.connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                List<ArtworksTag> tagList = new List<ArtworksTag>();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    List<String> tags = new List<String>();
                    int startYear = 0, endYear = 0;
                    //tags.Add(dataReader["fileName"] + "");
                    tags.Add(dataReader["titleOrigin"] + "");
                    tags.Add(dataReader["titleJ"] + "");
                    tags.Add(dataReader["artistE"] + "");
                    tags.Add(dataReader["artistJ"] + "");
                    tags.Add(dataReader["museumE"] + "");
                    tags.Add(dataReader["museumJ"] + "");
                    tags.Add(dataReader["countryE"] + "");
                    tags.Add(dataReader["countryJ"] + "");
                    tags.Add(dataReader["period1E"] + "");
                    tags.Add(dataReader["period1J"] + "");
                    tags.Add(dataReader["period2E"] + "");
                    tags.Add(dataReader["period2J"] + "");
                    tags.Add(dataReader["categoryJ"] + "");
                    tags.Add(dataReader["categoryE"] + "");
                    tags.Add(dataReader["genreE"] + "");
                    tags.Add(dataReader["genreJ"] + "");
                    if (dataReader["startYear"] == DBNull.Value)
                        startYear = 0;
                    else
                        startYear = (int)dataReader["startYear"];
                    if (dataReader["endYear"] == DBNull.Value)
                        endYear = startYear;
                    else
                        endYear = (int)dataReader["endYear"];
                    //Console.WriteLine(tags[1]);
                    tags.Add("Color");

                    ArtworksTag log = new ArtworksTag(tags, startYear, endYear);


                    tagList.Add(log);
                    //log.startYear = dataReader.GetInt32(dataReader.GetOrdinal("startYear"));
                    //log.startYear = dataReader.GetInt32(dataReader.GetOrdinal("endYear"));
                    // read color info
                    /*if (dataReader.GetFloat(dataReader.GetOrdinal("color_h")) != null && dataReader.GetFloat(dataReader.GetOrdinal("color_s")) != null && dataReader.GetFloat(dataReader.GetOrdinal("color_v")) != null && dataReader.GetFloat(dataReader.GetOrdinal("color_variance")) != null)
                    {
                        float h = dataReader.GetFloat(dataReader.GetOrdinal("color_h"));
                        float s = dataReader.GetFloat(dataReader.GetOrdinal("color_s"));
                        float v = dataReader.GetFloat(dataReader.GetOrdinal("color_v"));
                        float variance = dataReader.GetFloat(dataReader.GetOrdinal("variance"));
                        log.feature = new Vector3(h, s, v);
                        log.variance = variance;
                    }*/

                    fileTags[dataReader["fileName"] + ""] = log;

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                db.CloseConnection();

                
                //return list to be displayed
                return fileTags;
            }
            else
            {
                return fileTags;
            }
        }
    }
}
