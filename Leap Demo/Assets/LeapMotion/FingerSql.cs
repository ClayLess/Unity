using System;
using System.Collections;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Leap;
namespace LeapSql
{
    class FingerSql
    {
        public Finger finger;
        public MySqlConnection mscon;
        public FingerSql()
        {

        }
        public FingerSql(MySqlConnection msconnection)
        {
            mscon = msconnection;
            finger = new Finger();
        }
        public bool GetFingerFromDB(int id)
        {
            
            string msg_f = "select * from finger where finger_id=" + id;
            BoneSql bs = new BoneSql(mscon);
            Bone[] b = new Bone[4];
            for (int i = 0; i < 4; i++)
            {
                bs.GetBoneFromDB(id * 10 + i + 1);
                b[i] = bs.bone;
            }
           try
            {
                mscon.Open();
                MySqlCommand mscmd = new MySqlCommand(msg_f, mscon);
                MySqlDataReader reader = mscmd.ExecuteReader();
                if (reader.Read())
                {
                    Finger buffer = new Finger
                        (
                        1,//frame id
                        1,//hand id
                        1,//finger_id
                        10000,//timevisible
                        new Vector(reader.GetFloat(1), reader.GetFloat(2), reader.GetFloat(3)),//tipposition
                        new Vector(reader.GetFloat(4), reader.GetFloat(5), reader.GetFloat(6)),//tipVelocity
                        new Vector(reader.GetFloat(7), reader.GetFloat(8), reader.GetFloat(9)),//direction
                        new Vector(reader.GetFloat(10), reader.GetFloat(11), reader.GetFloat(12)),//stablizedtipposition
                        reader.GetFloat(13),//width
                        reader.GetFloat(14),//length
                        reader.GetBoolean(15),//isExtend
                        (Finger.FingerType)reader.GetInt32(16),//type
                        b[0],
                        b[1],
                        b[2],
                        b[3]
                        );
                    finger = buffer;
                }
                mscon.Close();
                return true;
            }
            catch
            {
                Console.WriteLine("answer not unique");
                finger = new Finger();
                return false;
            }
        }
        public bool AddFinger2DB(int id)
        {
            mscon.Open();
            BoneSql bs = new BoneSql(mscon);
            string msg =
                "insert into finger values(" + id + ","
                + finger.TipPosition.x + ","
                + finger.TipPosition.y + ","
                + finger.TipPosition.z + ","
                + finger.TipVelocity.x + ","
                + finger.TipVelocity.y + ","
                + finger.TipVelocity.z + ","
                + finger.Direction.x + ","
                + finger.Direction.y + ","
                + finger.Direction.z + ","
                + finger.StabilizedTipPosition.x + ","
                + finger.StabilizedTipPosition.y + ","
                + finger.StabilizedTipPosition.z + ","
                + finger.Width + ","
                + finger.Length + ","
                + finger.IsExtended + ","
                + (int)finger.Type + ");"
                ;
            MySqlCommand mscmd = new MySqlCommand(msg, mscon);
            if (mscmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine(mscmd.CommandText);
                Console.WriteLine("数据插入成功！");
            }
            mscon.Close();
            for (int i = 0; i < 4; i++)
            {
                bs.bone = finger.bones[i];
                bs.AddBone2DB(id * 10 + i + 1);
            }
            return true;
        }
    }
}
