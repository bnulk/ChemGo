using ChemGo.Data;
using ChemGo.Functions.Mecp;

namespace ChemGo.Drive
{
    partial class Drive_0101_Mecp
    {
        public void UpdateMecpData(ref Mecp mecp, ref SinglePoint singlePoint)
        {
            mecp = this.Mecp;
            UpdateSinglePointData(ref singlePoint);
        }

        private void UpdateSinglePointData(ref SinglePoint singlePoint)
        {
            singlePoint.energy = (Mecp.energy1 + Mecp.energy2) / 2;

            switch (data_ChemGo.inputFile.labels.control.coordinateType)
            {                
                case CoordinateType.zMatrix:
                    UpdateSinglePointData_ZMatrix(ref singlePoint);
                    break;
                case CoordinateType.Cartesian:
                    UpdateSinglePointData_Geometry(ref singlePoint);
                    break;
                default:
                    throw new DriveException("unknown InputFileType. \n @ChemGo.Drive.Drive_0101_Mecp;  UpdateSinglePointData(ref Data_ChemGo data_ChemGo):site1 CoordinateType Error");
            }

        }

        private void UpdateSinglePointData_Geometry(ref SinglePoint singlePoint)
        {
            singlePoint.geometry = data_ChemGo.mecp.geometry;
        }

        private void UpdateSinglePointData_ZMatrix(ref SinglePoint singlePoint)
        {
            singlePoint.zMatrix = data_ChemGo.mecp.zMatrix;
        }

    }
}
