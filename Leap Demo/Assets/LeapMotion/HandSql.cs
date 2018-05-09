using System;
using System.Collections;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Leap;
using System.Collections.Generic;

namespace LeapSql
{
    class HandSql
    {
        public Hand hand;
        public MySqlConnection mscon;
        public HandSql()
        {

        }
        public HandSql(MySqlConnection msconnection)
        {
            mscon = msconnection;
            hand = new Hand();
        }
        public bool GetHandFromDB(int id)
        {
            string msg = "select * from hand where hand_id=" + id;
            FingerSql fs = new FingerSql(mscon);
            List<Finger> fingers = new List<Finger>();
            
            for(int i=0;i<5;i++)
            {
                fs.GetFingerFromDB(id * 10 + i + 1);
                fingers.Add(fs.finger);
            }
            ArmSql arms = new ArmSql(mscon);
            arms.GetArmFromDB(id);
            try
            {
                mscon.Open();
                MySqlCommand mscmd = new MySqlCommand(msg, mscon);
                MySqlDataReader reader = mscmd.ExecuteReader();
                if(reader.Read())
                {
                    Hand buffer = new Hand
                        (
                        1,//frame_id
                        1,//hand_id
                        reader.GetFloat(1),//confidence
                        reader.GetFloat(2),//grabstrength
                        reader.GetFloat(3),//grabangle
                        reader.GetFloat(4),//pinchstrength
                        reader.GetFloat(5),//pinchdistance
                        reader.GetFloat(6),//palmwidth
                        reader.GetBoolean(7),//isleft
                        reader.GetFloat(8),//timevisibe
                        arms.arm,//arm
                        fingers,//finger
                        new Vector(reader.GetFloat(9), reader.GetFloat(10), reader.GetFloat(11)),//palmposition
                        new Vector(reader.GetFloat(12), reader.GetFloat(13), reader.GetFloat(14)),//stablizedpalmposition
                        new Vector(reader.GetFloat(15), reader.GetFloat(16), reader.GetFloat(17)),//palmvelocity
                        new Vector(reader.GetFloat(18), reader.GetFloat(19), reader.GetFloat(20)),//palmnormal
                        new LeapQuaternion(reader.GetFloat(21), reader.GetFloat(22), reader.GetFloat(22), reader.GetFloat(23)),//palmorientation
                        new Vector(reader.GetFloat(24), reader.GetFloat(25), reader.GetFloat(26)),//direction
                        new Vector(reader.GetFloat(27), reader.GetFloat(28), reader.GetFloat(29))//wristposition

                        );
                    hand = buffer;
                }
                mscon.Close();
                return true;
            }
            catch
            {
                Console.WriteLine("answer not unique");
                hand = new Hand();
                return false;
            }
            
           
        }
        public bool AddHand2DB(int id)
        {

            try
            {
                mscon.Open();
                
                string msg =
                    "insert into hand values(" + id + ","
                    + hand.Confidence + ","
                    + hand.GrabStrength + ","
                    + hand.GrabAngle + ","
                    + hand.PinchStrength + ","
                    + hand.PinchDistance + ","
                    + hand.PalmWidth + ","
                    + hand.IsLeft + ","
                    + hand.TimeVisible + ","
                    + hand.PalmPosition.x + ","
                    + hand.PalmPosition.y + ","
                    + hand.PalmPosition.z + ","
                    + hand.StabilizedPalmPosition.x + ","
                    + hand.StabilizedPalmPosition.y + ","
                    + hand.StabilizedPalmPosition.z + ","
                    + hand.PalmVelocity.x + ","
                    + hand.PalmVelocity.y + ","
                    + hand.PalmVelocity.z + ","
                    + hand.PalmNormal.x + ","
                    + hand.PalmNormal.y + ","
                    + hand.PalmNormal.z + ","
                    + hand.Rotation.x + ","
                    + hand.Rotation.y + ","
                    + hand.Rotation.z + ","
                    + hand.Rotation.w + ","
                    + hand.Direction.x + ","
                    + hand.Direction.y + ","
                    + hand.Direction.z + ","
                    + hand.WristPosition.x + ","
                    + hand.WristPosition.y + ","
                    + hand.WristPosition.z + ");"

                    ;
                MySqlCommand mscmd = new MySqlCommand(msg, mscon);
                if (mscmd.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine(mscmd.CommandText);
                    Console.WriteLine("数据插入成功！");
                }
                mscon.Close();
                FingerSql fs = new FingerSql(mscon);
                for (int i = 0; i < 5; i++)
                {
                    fs.finger=hand.Fingers[i];
                    fs.AddFinger2DB(id * 10 + i + 1);
                }
                ArmSql arms = new ArmSql(mscon);
                arms.arm = hand.Arm;
                arms.AddArm2DB(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void DeleteHand(int id)
        {
            mscon.Open();
            string msg =
                "delete from hand where hand_id = " + id + ";"
                + "delete from arm where arm_id = " + id + ";"
                + "delete from finger where finger_id >" + id * 10 + " and finger_id<" + (id * 10 + 9) + ";"
                + "delete from bone where bone_id >" + id * 100 + " and bone_id<" + (id * 100 + 99)+";";
            MySqlCommand mscmd = new MySqlCommand(msg, mscon);
            if (mscmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine(mscmd.CommandText);
                Console.WriteLine("删除成功！");
            }
            mscon.Close();
        }
        public void AddHand2DB()
        {
            int id;
            mscon.Open();
            string msg = "select max(hand_id) from hand;";
            MySqlCommand mscmd = new MySqlCommand(msg, mscon);
            MySqlDataReader reader = mscmd.ExecuteReader();
            if(reader.Read())
            {
                id = reader.GetInt32(0) + 1;
            }
            else
            {
                id = 1;
            }
            mscon.Close();
            AddHand2DB(id);
        }
    }
}
