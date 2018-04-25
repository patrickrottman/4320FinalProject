using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeChecker
{
    class StudentFile
    {
        protected String FileName;
        protected String FileContents;

        public StudentFile(String fileName, String fileContents)
        {
            this.FileName = fileName;
            this.FileContents = fileContents;
        }

        public String fileName
        {
            get
            {
                return FileName;
            }
            set
            {
                FileName = value;
            }
        }

        public String fileContents
        {
            get
            {
                return FileContents;
            }

            set
            {
                FileContents = value;
            }
        }

    }
}
