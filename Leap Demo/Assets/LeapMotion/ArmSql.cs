using System;
using System.Collections;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Leap;
namespace LeapSql
{
    class ArmSql
    {
        public Arm arm;  //arm object
        public MySqlConnection mscon;//Mysql server connection
        public ArmSql()
        {

        }
        public ArmSql(MySqlConnection msconnection)
        {
            mscon = msconnection;
            arm = new Arm();
        }
        public bool GetArmFromDB(int id)
        {
            mscon.Open();//
            string msg = "select * from arm where arm_id=" + id;
            MySqlCommand mscmd = new MySqlCommand(msg, mscon);
            MySqlDataReader reader = mscmd.ExecuteReader();
            int counter=0;
            if (reader.Read())
            {
                if (counter > 0)
                {
                    Console.WriteLine("answer not unique");
                    arm = new Arm();
                    return false;
                }
                Arm buffer = new Arm(
                    new Vector(reader.GetFloat(1), reader.GetFloat(2), reader.GetFloat(3)),
                    new Vector(reader.GetFloat(4), reader.GetFloat(5), reader.GetFloat(6)),
                    new Vector(reader.GetFloat(7), reader.GetFloat(8), reader.GetFloat(9)),
                    new Vector(reader.GetFloat(10), reader.GetFloat(11), reader.GetFloat(12)),
                    reader.GetFloat(13),
                    reader.GetFloat(14),
                    new LeapQuaternion(reader.GetFloat(15), reader.GetFloat(16), reader.GetFloat(17), reader.GetFloat(18))
                    );
                counter++;
                arm = buffer;
            }
            mscon.Close();//
            return true;
        }
        public bool AddArm2DB(int id)
        {
            mscon.Open();
            string msg1 = "select count(*) from arm";
            MySqlCommand mscmd = new MySqlCommand(msg1, mscon);
            MySqlDataReader reader = mscmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                string msg2 =
                    "insert into arm values(" + id + ","
                    + arm.ElbowPosition.x + ","
                    + arm.ElbowPosition.y + ","
                    + arm.ElbowPosition.z + ","
                    + arm.WristPosition.x + ","
                    + arm.WristPosition.y + ","
                    + arm.WristPosition.z + ","
                    + arm.Center.x + ","
                    + arm.Center.y + ","
                    + arm.Center.z + ","
                    + arm.Direction.x + ","
                    + arm.Direction.y + ","
                    + arm.Direction.z + ","
                    + arm.Length + ","
                    + arm.Width + ","
                    + arm.Rotation.x + ","
                    + arm.Rotation.y + ","
                    + arm.Rotation.z + ","
                    + arm.Rotation.w + ");"
                    ;
                mscmd = new MySqlCommand(msg2, mscon);
            }
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
