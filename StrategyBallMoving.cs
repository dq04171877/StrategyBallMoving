using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using xna = Microsoft.Xna.Framework;
using URWPGSim2D.Common;
using URWPGSim2D.StrategyLoader;

namespace URWPGSim2D.Strategy
{
    public class Strategy : MarshalByRefObject, IStrategy
    {
        #region 前言
        #region reserved code never be changed or removed
        /// <summary>
        /// override the InitializeLifetimeService to return null instead of a valid ILease implementation
        /// to ensure this type of remote object never dies
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            //return base.InitializeLifetimeService();
            return null; // makes the object live indefinitely
        }
        #endregion

        /// <summary>
        /// 决策类当前对象对应的仿真使命参与队伍的决策数组引用 第一次调用GetDecision时分配空间
        /// </summary>
        private Decision[] decisions = null;

        /// <summary>
        /// 获取队伍名称 在此处设置参赛队伍的名称
        /// </summary>
        /// <returns>队伍名称字符串</returns>
        public string GetTeamName()
        {
            return "水中搬运 Test Team";
        }


        #endregion

        #region 函数

        #region 该函数返回任意两点间（当前点与目标点。方向为当前点指向目标点）连线与X轴正向夹角（弧度制）

        /// </summary>add by 10级团队
        /// <param name="cur_x">当前点X坐标</param>
        /// <param name="cur_z">当前点Z坐标</param>
        /// <param name="dest_x">目标点X坐标</param>
        /// <param name="dest_z">目标点Z坐标</param>
        /// <param name="?"></param>
        /// <returns>与X轴正向夹角</returns>
        float GetAnyangle(float cur_x, float cur_z, float dest_x, float dest_z)
        {
            float curangle, anyangel;
            curangle = (float)(Math.Abs(Math.Atan((cur_x - dest_x) / (cur_z - dest_z))));
            if (cur_x > dest_x)
            {
                if (cur_z < dest_z)
                    anyangel = (float)(curangle + Math.PI / 2);
                else
                    anyangel = (float)(-(curangle + Math.PI / 2));

            }
            else
            {

                if (cur_z < dest_z)
                    anyangel = (float)(Math.PI / 2 - curangle);
                else
                    anyangel = (float)(-(Math.PI / 2 - curangle));

            }
            return anyangel;


        }
        #endregion
        #region 此函数让i鱼游向指定点    by 王闯
        /// <summary>
        /// 此函数让i鱼游向指定点
        /// </summary>
        /// <param name="mission"></param>
        /// <param name="fish"></param>
        /// <param name="i"></param>
        /// <param name="dest_x"></param>
        /// <param name="dest_z"></param>
        /// <param name="teamId"></param>
        void SwimToDest(Mission mission, ref Decision[] fish, int i, float dest_x, float dest_z, int teamId)
        {


            decisions[i].TCode = TransfromAngletoTCode(Getxzdangle(mission.TeamsRef[teamId].Fishes[i].PositionMm.X, mission.TeamsRef[teamId].Fishes[i].PositionMm.Z, dest_x, dest_z, mission.TeamsRef[teamId].Fishes[i].BodyDirectionRad));
            decisions[i].VCode = 5;
            int angle = (int)RedToAngle(Getxzdangle(mission.TeamsRef[teamId].Fishes[i].PositionMm.X, mission.TeamsRef[teamId].Fishes[i].PositionMm.Z, dest_x, dest_z, mission.TeamsRef[teamId].Fishes[i].BodyDirectionRad));

            if ((angle > -20) && (angle < 1))
            {
                decisions[i].TCode = 7;
                decisions[i].VCode = 14;
            }
            else if ((angle > -40) && (angle < -20))
            {
                decisions[i].TCode = 2;
                decisions[i].VCode = 2;
            }
            else if ((angle > -60) && (angle < -40))
            {
                decisions[i].TCode = 1;
                decisions[i].VCode = 2;
            }
            else if ((angle > -180) && (angle < -60))
            {
                decisions[i].TCode = 0;
                decisions[i].VCode = 1;
            }
            else if ((angle > 1) && (angle < 20))
            {
                decisions[i].TCode = 7;
                decisions[i].VCode = 14;
            }

            else if ((angle > 20) && (angle < 40))
            {
                decisions[i].TCode = 12;
                decisions[i].VCode = 3;
            }
            else if ((angle > 40) && (angle < 60))
            {
                decisions[i].TCode = 13;
                decisions[i].VCode = 2;
            }
            else if ((angle > 60) && (angle < 180))
            {
                decisions[i].TCode = 14;
                decisions[i].VCode = 1;
            }

        }
        #endregion
        #region 此函数根据待扭转的角度计算角速度TCode
        int TransfromAngletoTCode(float angvel)
        {
            if (0 == angvel)
            {
                return 7;
            }
            else if (angvel < 0)
            {
                if (-0.0269 <= angvel && 0 > angvel)
                {
                    if ((0 - angvel) >= (angvel + 0.0269))
                        return 6;
                    else
                        return 7;

                }
                else if (-0.04508 <= angvel && -0.0269 > angvel)
                {
                    if ((-0.0269 - angvel) >= (angvel + 0.04508))
                        return 5;
                    else
                        return 6;

                }
                else if (-0.071013 <= angvel && -0.04508 > angvel)
                {
                    if ((-0.04508 - angvel) >= (angvel + 0.071013))
                        return 4;
                    else
                        return 5;
                }
                else if (-0.09953 <= angvel && -0.071013 > angvel)
                {
                    if ((-0.071013 - angvel) >= (angvel + 0.09953))
                        return 3;
                    else
                        return 4;
                }
                else if (-0.1265 <= angvel && -0.09953 > angvel)
                {
                    if ((-0.09953 - angvel) >= (angvel + 0.1265))
                        return 2;
                    else
                        return 3;
                }
                else if (-0.1680 <= angvel && -0.1265 > angvel)
                {
                    if ((-0.1265 - angvel) >= (angvel + 0.1680))
                        return 1;
                    else
                        return 2;
                }
                else if (-0.20424 <= angvel && -0.1680 > angvel)
                {
                    if ((-0.1680 - angvel) >= (angvel + 0.20424))
                        return 0;
                    else
                        return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (0.0269 >= angvel && 0 < angvel)
                {
                    if (angvel - 0 > 0.0269 - angvel)
                        return 8;
                    else
                        return 7;

                }
                else if (0.04508 >= angvel && 0.0269 < angvel)
                {
                    if (angvel - 0.0269 > 0.04508 - angvel)
                        return 9;
                    else
                        return 8;
                }
                else if (0.071013 >= angvel && 0.04508 < angvel)
                {
                    if (angvel - 0.04508 > 0.071013 - angvel)
                        return 10;
                    else
                        return 9;
                }
                else if (0.09953 >= angvel && 0.071013 < angvel)
                {
                    if (angvel - 0.071013 > 0.09953 - angvel)
                        return 11;
                    else
                        return 10;
                }
                else if (0.1265 >= angvel && 0.09953 < angvel)
                {
                    if (angvel - 0.09953 > 0.1265 - angvel)
                        return 12;
                    else
                        return 11;
                }
                else if (0.1680 >= angvel && 0.1265 < angvel)
                {
                    if (angvel - 0.1265 > 0.1680 - angvel)
                        return 13;
                    else
                        return 12;
                }
                else if (0.1977 >= angvel && 0.1680 < angvel)
                {
                    if (angvel - 0.1680 > 0.1977 - angvel)
                        return 14;
                    else
                        return 13;
                }
                else
                {
                    return 14;
                }

            }
        }
        #endregion
        #region//该函数返回鱼要到达定点所需转的角度,cur_x-当前点X坐标,cur_z-当前点Z坐标,dest_x-目标点X坐标,dest_z-目标点Y坐标,返回需要偏转角度
        float Getxzdangle(float cur_x, float cur_z, float dest_x, float dest_z, float fish_rad)
        {
            float curangle,xzdangle=7;
            curangle = (float)(Math.Abs(Math.Atan((cur_x - dest_x) / (cur_z - dest_z))));
            if ((cur_x > dest_x) && (cur_z > dest_z))//以球为中心，当鱼在球右下角
            {
                if (fish_rad > (-(Math.PI / 2 + curangle)) && fish_rad < (Math.PI / 2 - curangle))
                {
                    xzdangle = (float)(Math.PI / 2 + curangle + fish_rad);
                    xzdangle = -xzdangle;
                }
                else if (fish_rad > (Math.PI / 2 - curangle) && fish_rad < Math.PI)
                {
                    xzdangle = (float)(Math.PI * 1.5 - fish_rad - curangle);
                }
                else if (fish_rad < (-(Math.PI / 2 + curangle)) && fish_rad > -Math.PI)
                {
                    xzdangle = (float)(-Math.PI / 2 - curangle - fish_rad);
                }
            }
            else if ((cur_x > dest_x) && (cur_z < dest_z))//以球为中心，当鱼在球右上角
            {
                if (fish_rad < (Math.PI / 2 + curangle) && (-(Math.PI / 2 - curangle)) < fish_rad)
                {
                    xzdangle = (float)(Math.PI / 2 + curangle - fish_rad);
                }
                else if ((-(Math.PI / 2 - curangle)) > fish_rad && fish_rad > -Math.PI)
                {
                    xzdangle = (float)(Math.PI * 1.5 + fish_rad - curangle);
                    xzdangle = -xzdangle;
                }
                else if (fish_rad > (Math.PI / 2 + curangle) && fish_rad < Math.PI)
                {
                    xzdangle = (float)(fish_rad - curangle - Math.PI / 2);
                    xzdangle = -xzdangle;
                }
            }
            else if ((cur_x < dest_x) && (cur_z > dest_z))//以球为中心，当鱼在球左下角
            {
                if (fish_rad > -(Math.PI / 2 - curangle) && fish_rad < (Math.PI / 2 + curangle))
                {
                    xzdangle = (float)(Math.PI / 2 + fish_rad - curangle);
                    xzdangle = -xzdangle;
                }
                else if (fish_rad < -(Math.PI / 2 - curangle) && fish_rad > -Math.PI)
                {
                    xzdangle = (float)(curangle - fish_rad - Math.PI / 2);
                }
                else if (fish_rad > (Math.PI / 2 + curangle) && fish_rad < Math.PI)
                {
                    xzdangle = (float)(Math.PI * 1.5 + curangle - fish_rad);
                }
            }
            else if ((cur_x < dest_x) && (cur_z < dest_z))//以球为中心，当鱼在球左上角
            {
                if (fish_rad < (Math.PI / 2 - curangle) && fish_rad > -(Math.PI / 2 + curangle))
                {
                    xzdangle = (float)(Math.PI / 2 - curangle - fish_rad);

                }
                else if (fish_rad > (Math.PI / 2 - curangle) && fish_rad < Math.PI)
                {
                    xzdangle = (float)(fish_rad + curangle - Math.PI / 2);
                    xzdangle = -xzdangle;
                }
                else if (fish_rad < -(Math.PI / 2 + curangle) && fish_rad > -Math.PI)
                {
                    xzdangle = (float)(Math.PI * 1.5 + fish_rad + curangle);
                    xzdangle = -xzdangle;
                }
            }

            return xzdangle;
        }
        #endregion    
        #region 将弧度转换为角度
        /// <summary>将弧度转换为角度
        /// 
        /// </summary>add by 兽之哀
        /// <param name="red"></param>弧度
        /// <returns></returns>角度
        float RedToAngle(float red)
        {
            return ((float)((red / Math.PI) * 180));
        }
        #endregion
        #region 结果判断函数
        void result(Mission mission)
        {
            ball_in[0] = Convert.ToInt32(mission.HtMissionVariables["Ball0InHole"]);
            ball_in[1] = Convert.ToInt32(mission.HtMissionVariables["Ball1InHole"]);
            ball_in[2] = Convert.ToInt32(mission.HtMissionVariables["Ball2InHole"]);
            ball_in[3] = Convert.ToInt32(mission.HtMissionVariables["Ball3InHole"]);
            ball_in[4] = Convert.ToInt32(mission.HtMissionVariables["Ball4InHole"]);
            ball_in[5] = Convert.ToInt32(mission.HtMissionVariables["Ball5InHole"]);
        }
        #endregion
        #region 取得现在鱼头和水球是否接触
        bool isHeadTouchBall(Mission mission,int teamId,int fish,int ball) {
            xna.Vector3 head = mission.TeamsRef[teamId].Fishes[fish].PolygonVertices[0];
            xna.Vector3 ball_pos=mission.EnvRef.Balls[ball].PositionMm;
            double distance= Math.Sqrt( Math.Pow((head.X - ball_pos.X), 2)+Math.Pow((head.Z - ball_pos.Z), 2));
           if(distance >=mission.EnvRef.Balls[ball].RadiusMm+10){
               return false;
           }
           return true ;
        }
        #endregion
        #region 取得现在鱼头右侧和水球是否接触
        bool isHeadRightTouchBall(Mission mission, int teamId, int fish, int ball)
        {
            xna.Vector3 head_right = mission.TeamsRef[teamId].Fishes[fish].PolygonVertices[6];
            xna.Vector3 ball_pos = mission.EnvRef.Balls[ball].PositionMm;
            double distance = Math.Sqrt(Math.Pow((head_right.X - ball_pos.X), 2) + Math.Pow((head_right.Z - ball_pos.Z), 2));
            if (distance >= mission.EnvRef.Balls[ball].RadiusMm + 2)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region 取得现在鱼头左侧和水球是否接触
        bool isHeadLeftTouchBall(Mission mission, int teamId, int fish, int ball)
        {
            xna.Vector3 head_left = mission.TeamsRef[teamId].Fishes[fish].PolygonVertices[1];
            xna.Vector3 ball_pos = mission.EnvRef.Balls[ball].PositionMm;
            double distance = Math.Sqrt(Math.Pow((head_left.X - ball_pos.X), 2) + Math.Pow((head_left.Z - ball_pos.Z), 2));
            if (distance >= mission.EnvRef.Balls[ball].RadiusMm + 2)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region 取得当前鱼体中心位置
        xna.Vector3 fishPosition(Mission mission,int teamId,int i)
        {
            return mission.TeamsRef[teamId].Fishes[i].PositionMm;
        }
        #endregion 
        #region 取得现在鱼体中心和指定点是否接触
        bool isFishTouchPoint(Mission mission, int teamId, int fishi, xna.Vector3 point)
        {
            xna.Vector3 fishPos=fishPosition(mission,teamId,fishi);
            if (fishPos.X - hole[0].X < 2 && fishPos.Z-hole[0].Z<2)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region 取得现在鱼体方向是否在期望方向上
        bool isOnDirection(Mission mission,int teamId,int fishi,double expRad){
            if (Math.Abs(mission.TeamsRef[teamId].Fishes[fishi].BodyDirectionRad - expRad) < 0.2) {
                return true;
            }
            return false;
        }
        #endregion
        #region 是否转过180度
        bool isRotateToPos(Mission mission,int teamId){
            bool isTouch = isHeadRightTouchBall(mission, teamId, 0, 5) || isHeadLeftTouchBall(mission, teamId, 0, 5);
            bool isToPi = isOnDirection(mission, teamId, 0, (-1)* Math.PI) || isOnDirection(mission, teamId, 0, Math.PI);
            return isTouch&&isToPi;
        }
        #endregion
        #region 斜边长
        float GetLengthToLength(float cur_a, float cur_b)
        {
            return (float)Math.Sqrt(Math.Pow((cur_a), 2) + Math.Pow((cur_b), 2));
        }
        #endregion
        #region 测试
        void testCopy(Mission mission, int teamId, int fishi, float FBZXangle1)
        {
            decisions[fishi].TCode = TransfromAngletoTCode(FBZXangle1);
            int TV = TransfromAngletoTCode(FBZXangle1);
            int TEMP = Math.Abs(7 - TV);
            switch ((int)TEMP)
            {
                case 0: decisions[fishi].VCode = 14;
                    break;
                case 1: decisions[fishi].VCode = 13;
                    break;
                case 2: decisions[fishi].VCode = 12;
                    break;
                case 3: decisions[fishi].VCode = 11;
                    break;
                case 4: decisions[fishi].VCode = 10;
                    break;
                case 5: decisions[fishi].VCode = 9;
                    break;
                case 6: decisions[fishi].VCode = 8;
                    break;
                case 7: decisions[fishi].VCode = 2;
                    break;
                default:
                    decisions[fishi].VCode = 1;
                    break;
            }
        }

        #endregion
        #endregion

        #region 定义
        #region 定义6个地标中心坐标

        static xna.Vector3[] hole = new xna.Vector3[6];
        void setHole()
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    int l = i * (i - 1) / 2 + j - 1;
                    hole[l].Y = 0;
                    hole[l].X = -(i * 3 - 3) * 80;
                    hole[l].Z = -(i - 1) * 2 * 80 + 4 * (j - 1) * 80;
                }
            }
        }

        #endregion
        #region 获得6个球的位置保存在ball_loc数组
        static xna.Vector3[] ball_loc = new xna.Vector3[6];//球的位置坐标
        void setBall_loc(Mission misson) {
            for (int i = 0; i < 6; i++) {
                ball_loc[i] = misson.EnvRef.Balls[i].PositionMm;
            }
        }

        #endregion

        #region 变量
        static int times = 0;
        static int stage; //阶段标志位
        static int isArrive=0;
        static Boolean ori = false;
        static int[] ball_in = new int[6]; //1-进球，0-没进，其他无意义
        
      

        

        
        
        #endregion


        #endregion

        #region 正文


        /// <summary>
        /// 获取当前仿真使命（比赛项目）当前队伍所有仿真机器鱼的决策数据构成的数组
        /// </summary>
        /// <param name="mission">服务端当前运行着的仿真使命Mission对象</param>
        /// <param name="teamId">当前队伍在服务端运行着的仿真使命中所处的编号 
        /// 用于作为索引访问Mission对象的TeamsRef队伍列表中代表当前队伍的元素</param>
        /// <returns>当前队伍所有仿真机器鱼的决策数据构成的Decision数组对象</returns>
        public Decision[] GetDecision(Mission mission, int teamId)
        {
            // 决策类当前对象第一次调用GetDecision时Decision数组引用为null
            if (decisions == null)
            {// 根据决策类当前对象对应的仿真使命参与队伍仿真机器鱼的数量分配决策数组空间
                decisions = new Decision[mission.CommonPara.FishCntPerTeam];
            }
            #region 决策计算过程 需要各参赛队伍实现的部分
            #region 策略编写帮助信息
            //====================我是华丽的分割线====================//
            //======================策略编写指南======================//
            //1.策略编写工作直接目标是给当前队伍决策数组decisions各元素填充决策值
            //2.决策数据类型包括两个int成员，VCode为速度档位值，TCode为转弯档位值
            //3.VCode取值范围0-14共15个整数值，每个整数对应一个速度值，速度值整体但非严格递增
            //有个别档位值对应的速度值低于比它小的档位值对应的速度值，速度值数据来源于实验
            //4.TCode取值范围0-14共15个整数值，每个整数对应一个角速度值
            //整数7对应直游，角速度值为0，整数6-0，8-14分别对应左转和右转，偏离7越远，角度速度值越大
            //5.任意两个速度/转弯档位之间切换，都需要若干个仿真周期，才能达到稳态速度/角速度值
            //目前运动学计算过程决定稳态速度/角速度值接近但小于目标档位对应的速度/角速度值
            //6.决策类Strategy的实例在加载完毕后一直存在于内存中，可以自定义私有成员变量保存必要信息
            //但需要注意的是，保存的信息在中途更换策略时将会丢失
            //====================我是华丽的分割线====================//
            //=======策略中可以使用的比赛环境信息和过程信息说明=======//
            //场地坐标系: 以毫米为单位，矩形场地中心为原点，向右为正X，向下为正Z
            //            负X轴顺时针转回负X轴角度范围为(-PI,PI)的坐标系，也称为世界坐标系
            //mission.CommonPara: 当前仿真使命公共参数
            //mission.CommonPara.FishCntPerTeam: 每支队伍仿真机器鱼数量
            //mission.CommonPara.MsPerCycle: 仿真周期毫秒数
            //mission.CommonPara.RemainingCycles: 当前剩余仿真周期数
            //mission.CommonPara.TeamCount: 当前仿真使命参与队伍数量
            //mission.CommonPara.TotalSeconds: 当前仿真使命运行时间秒数
            //mission.EnvRef.Balls: 
            //当前仿真使命涉及到的仿真水球列表，列表元素的成员意义参见URWPGSim2D.Common.Ball类定义中的注释
            //mission.EnvRef.FieldInfo: 
            //当前仿真使命涉及到的仿真场地，各成员意义参见URWPGSim2D.Common.Field类定义中的注释
            //mission.EnvRef.ObstaclesRect: 
            //当前仿真使命涉及到的方形障碍物列表，列表元素的成员意义参见URWPGSim2D.Common.RectangularObstacle类定义中的注释
            //mission.EnvRef.ObstaclesRound:
            //当前仿真使命涉及到的圆形障碍物列表，列表元素的成员意义参见URWPGSim2D.Common.RoundedObstacle类定义中的注释
            //mission.TeamsRef[teamId]:
            //决策类当前对象对应的仿真使命参与队伍（当前队伍）
            //mission.TeamsRef[teamId].Para:
            //当前队伍公共参数，各成员意义参见URWPGSim2D.Common.TeamCommonPara类定义中的注释
            //mission.TeamsRef[teamId].Fishes:
            //当前队伍仿真机器鱼列表，列表元素的成员意义参见URWPGSim2D.Common.RoboFish类定义中的注释
            //mission.TeamsRef[teamId].Fishes[i].PositionMm和PolygonVertices[0],BodyDirectionRad,VelocityMmPs,
            //                                   AngularVelocityRadPs,Tactic:
            //当前队伍第i条仿真机器鱼鱼体矩形中心和鱼头顶点在场地坐标系中的位置（用到X坐标和Z坐标），鱼体方向，速度值，
            //                                   角速度值，决策值
            //====================我是华丽的分割线====================//
            //========================典型循环========================//
            //for (int i = 0; i < mission.CommonPara.FishCntPerTeam; i++)
            //{
            //  decisions[i].VCode = 0; // 静止
            //  decisions[i].TCode = 7; // 直游
            //}
            //====================我是华丽的分割线====================//
            #endregion
            //请从这里开始编写代码
            if(ori==false){
                ori = true;
                setHole();
            }
            setBall_loc(mission);
         //   if (isArrive==1||(isFishTouchPoint(mission,teamId,1,hole[0])&&isOnDirection(mission,teamId,1,Math.PI)))
             
         //   StrategyHelper.Helpers.PoseToPose(ref decisions[1], mission.TeamsRef[teamId].Fishes[1], hole[0],(float) Math.PI, 30.0f, 20.0f, mission.CommonPara.MsPerCycle, ref times);

            if (isArrive!=0 || isRotateToPos(mission, teamId))
            {

                if (isArrive == 1||isArrive == 0)
                {
                    StrategyHelper.Helpers.Dribble(ref decisions[0], mission.TeamsRef[teamId].Fishes[0], ball_loc[5], (float)Math.PI,
                                                                10, 15, 130, 14, 14, 20, 100, false);
                    isArrive = 1;
                }
                if (isArrive == 2)
                {
                    decisions[0].TCode = 7;
                    decisions[0].VCode = 14;

                }
                if (isHeadTouchBall(mission, teamId, 0, 5))
                {
                    isArrive = 2;
                }
                else
                {
                    isArrive = 1;
                }
                   
            }
            else
            {

                SwimToDest(mission, ref decisions, 0, ball_loc[5].X + 100, ball_loc[5].Z + 100, teamId);
            }
            float FX,FZ,BX,BZ,BX1,BZ1,PiontZ0,LengthBX0,LengthBZ0,LengthBXZ0,FBZXangle1;
            FX = mission.TeamsRef[teamId].Fishes[0].PositionMm.X;//鱼的横轴坐标
            FZ = mission.TeamsRef[teamId].Fishes[0].PositionMm.Z;//鱼的纵轴坐标

            BX = mission.EnvRef.Balls[0].PositionMm.X;//
            BZ = mission.EnvRef.Balls[0].PositionMm.Z;//球的纵轴坐标


            LengthBX0 = (float)-950 - BX;    //X轴长
            LengthBZ0 = (float)400 - BZ;         //(1500,0)Z轴长度
            LengthBXZ0 = GetLengthToLength(LengthBX0, LengthBZ0);   //斜边长

            PiontZ0 = ((float)28 / LengthBXZ0) * (LengthBZ0);

            BX1 = BX;
            BZ1 = BZ - PiontZ0 - 100;                  //绕球顶球点Z坐标
            FBZXangle1 = Getxzdangle(FX, FZ, BX1, BZ1, mission.TeamsRef[0].Fishes[0].BodyDirectionRad);     //正顶球点
            testCopy(mission, teamId, 1, FBZXangle1);
          






            //      StrategyHelper.Helpers.Dribble(ref decisions[0], mission.TeamsRef[teamId].Fishes[0], ball_loc[5], 3.1415f,
            //                                             10, 15, 130, 14, 14, 20, 100, false );


            //           StrategyHelper.Helpers.PoseToPose(ref decisions[0], mission.TeamsRef[teamId].Fishes[0], hole[0], 3.14f, 30.0f, 20.0f, mission.CommonPara.MsPerCycle, ref times);
            //          times = 0;
            return decisions;
            
            #endregion
        }
        #endregion
    }
}
