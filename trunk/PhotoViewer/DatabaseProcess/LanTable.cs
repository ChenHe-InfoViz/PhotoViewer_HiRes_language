using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PhotoInfo;
using System.IO;

namespace database
{
    class LanTable: TableProcessor
    {
        DBConnect db = new DBConnect();
        public Dictionary<string, PhotoTag> select(List<string> fileName)
        {
            Dictionary<string, PhotoTag> fileTags = new Dictionary<string, PhotoTag>();
            StreamReader reader = new StreamReader("lan.xml");
            var d = reader.ReadToEnd();

            return ArtworksTag.FromXml(d);





            string query = "SELECT * FROM language1 WHERE English = '" + fileName[0] + "'";
            for (int i = 1; i < fileName.Count; i++)
            {
                query += " or English = '" + fileName[i] + "'";
            }
            query = query.Replace(@"\", @"\\");
            //Open connection
            if (db.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, db.connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                List<LanTag> tagList = new List<LanTag>();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    List<String> tags = new List<String>();
                    string soundexEng = (string)dataReader["SoundexEng"];
                    string soundexRomaji = (string)dataReader["SoundexRomanji"];
                    tags.Add((string)dataReader["English"]);
                    tags.Add(dataReader["Japanese"] + "");
                    tags.Add(dataReader["Romanji"] + "");

                    LanTag log = new LanTag(tags, soundexEng, soundexRomaji);

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

                    fileTags[(string)dataReader["English"]] = log;

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                db.CloseConnection();
                var xml = ArtworksTag.ExportXml(fileTags);
                StreamWriter writer = new StreamWriter("color.xml");
                writer.Write(xml);
                writer.Close();

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
