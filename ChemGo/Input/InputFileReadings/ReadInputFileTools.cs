using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Input.InputFileReadings
{
    static class ReadInputFileTools
    {
        /// <summary>
        /// 获取任务类型
        /// </summary>
        /// <param name="strTask">字符串任务类型</param>
        /// <returns>任务类型</returns>
        public static Task GetTask(string strTask)
        {
            Task task;
            switch (strTask)
            {
                case "sp":
                    task = Task.sp;
                    break;
                case "min":
                    task = Task.min;
                    break;
                case "ts":
                    task = Task.ts;
                    break;
                case "mecp":
                    task = Task.mecp;
                    break;
                default:
                    task = Task.unknown;
                    break;
            }
            return task;
        }

        /// <summary>
        /// 获取坐标类型
        /// </summary>
        /// <param name="strCoordinateType">字符串型坐标类型</param>
        /// <returns>坐标类型</returns>
        public static CoordinateType GetCoordinateType(string strCoordinateType)
        {
            CoordinateType coordinateType;
            switch (strCoordinateType)
            {
                case "zmatrix":
                    coordinateType = CoordinateType.zMatrix;
                    break;
                case "cartesian":
                    coordinateType = CoordinateType.Cartesian;
                    break;
                default:
                    coordinateType = CoordinateType.Cartesian;
                    break;
            }
            return coordinateType;
        }
    }
}
