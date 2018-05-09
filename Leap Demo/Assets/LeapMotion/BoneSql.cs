using System;
using System.Collections;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Leap;
namespace LeapSql
{
    class BoneSql
    {
        public Bone bone;  //bone object
        public MySqlConnection mscon;//Mysql server connection
        public BoneSql()
        {

        }
        public BoneSql(MySqlConnection msconnection)
        {
            mscon = msconnection;
            bone = new Bone();
        }
        public bool GetBoneFromDB(int id)
        {
            mscon.Open();//
            string msg = "select * from bone where bone_id=" + id;
            MySqlCommand mscmd = new MySqlCommand(msg, mscon);
            MySqlDataReader reader = mscmd.ExecuteReader();
            int counter = 0;
            if (reader.Read())
            {
                if (counter > 0)
                {
                    Console.WriteLine("answer not unique");
                    bone = new Bone();
                    return false;
                }
                Bone.BoneType bt_buffer = (Bone.BoneType)reader.GetInt32(15);
                Bone buffer = new Bone(
                    new Vector(reader.GetFloat(1), reader.GetFloat(2), reader.GetFloat(3)),
                    new Vector(reader.GetFloat(4), reader.GetFloat(5), reader.GetFloat(6)),
                    new Vector(reader.GetFloat(7), reader.GetFloat(8), reader.GetFloat(9)),
                    new Vector(reader.GetFloat(10), reader.GetFloat(11), reader.GetFloat(12)),
                    reader.GetFloat(13),
                    reader.GetFloat(14),
                    bt_buffer,
                    new LeapQuaternion(reader.GetFloat(16), reader.GetFloat(17), reader.GetFloat(18), reader.GetFloat(19))
                );
                counter++;
                bone = buffer;
            }
            mscon.Close();//
            return true;
        }
        public bool AddBone2DB(int id)
        {
            mscon.Open();
            string msg2 =
                "insert into bone values(" + id + ","
                + bone.PrevJoint.x + ","
                + bone.PrevJoint.y + ","
                + bone.PrevJoint.z + ","
                + bone.NextJoint.x + ","
                + bone.NextJoint.y + ","
                + bone.NextJoint.z + ","
                + bone.Center.x + ","
                + bone.Center.y + ","
                + bone.Center.z + ","
                + bone.Direction.x + ","
                + bone.Direction.y + ","
                + bone.Direction.z + ","
                + bone.Length + ","
                + bone.Width + ","
                + (int)bone.Type +","
                + bone.Rotation.x + ","
                + bone.Rotation.y + ","
                + bone.Rotation.z + ","
                + bone.Rotation.w + ");"
                ;
            MySqlCommand mscmd = new MySqlCommand(msg2, mscon);
            if (mscmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine(mscmd.CommandText);
                Console.WriteLine("数据插入成功！");
            }
            mscon.Close();
            return true;
        }
    }

}
