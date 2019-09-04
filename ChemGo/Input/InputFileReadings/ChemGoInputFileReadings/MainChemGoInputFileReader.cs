using System;
using System.Collections.Generic;
using System.Text;
using ChemGo.Data;

namespace ChemGo.Input.InputFileReadings.ChemGoInputFileReadings
{
    class MainChemGoInputFileReader
    {
        private List<string> inputList;

        private InputFile inputFile;

        /// <summary>
        /// ChemGo标准格式之输入文件
        /// </summary>
        public InputFile InputFile { get => inputFile; set => inputFile = value; }



        public MainChemGoInputFileReader(List<string> inputList)
        {
            this.inputList = inputList;
        }

        public void Run()
        {
            ChemGoInfoReader chemGoInfoReader = new ChemGoInfoReader(inputList);
            chemGoInfoReader.Run();
            this.inputFile = chemGoInfoReader.InputFile;
        }


    }
}
