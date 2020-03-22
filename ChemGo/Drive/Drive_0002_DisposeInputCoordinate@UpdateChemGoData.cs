using System;
using System.Text;
using ChemGo.Data;
using ChemGo.Functions.DisposeInputCoordinate;

namespace ChemGo.Drive
{
    partial class Drive_0002_DisposeInputCoordinate
    {
        public void UpdateChemGoData(ref Data_ChemGo data_ChemGo)
        {
            data_ChemGo.singlePoint.numberOfAtoms = this.numberOfAtoms;
            data_ChemGo.singlePoint.geometry = this.geometry;
            data_ChemGo.singlePoint.zMatrix = this.zMatrix;
        }
    }
}
