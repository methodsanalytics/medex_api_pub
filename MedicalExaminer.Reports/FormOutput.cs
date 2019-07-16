using MedicalExaminer.Models;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stubble.Core.Builders;

namespace MedicalExaminer.Reports
{
    public class FormOutput
    {
        private readonly Examination _examination;

        public FormOutput(Examination examination)
            :base()
        {
            _examination = examination;
        }

        private string ParseTemplate(string template)
        {
            var stubble = new StubbleBuilder().Build();

            var output = stubble.Render(template, _examination);

            return output;
        }


        public byte[] GetBytes()
        {
            var _templatePath = @"Forms/Templates/coroner-referral-form.odt";

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _templatePath);

            //var examinationDoc = JsonConvert.SerializeObject(_examination);

            var contentFile = "content.xml";

            // TODO: permissions

            var templateStream = System.IO.File.ReadAllBytes(path);

            byte[] outputStream;

            using (var memoryStream = new MemoryStream(templateStream))
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
                {
                    var contentsEntry = archive.Entries.FirstOrDefault(e => e.Name == contentFile);
                    if (contentsEntry != null)
                    {
                        string contents;

                        using (var contentReader = new StreamReader(contentsEntry.Open()))
                        {
                            contents = contentReader.ReadToEnd();

                            contents = ParseTemplate(contents);

                           // contents.Replace()

                            //contents = contents.Replace("[Forename]", "REPLACED!!");
                        }

                        using (var contentWriter = new StreamWriter(contentsEntry.Open()))
                        {
                            contentWriter.Write(contents);
                        }
                    }
                }

                outputStream = memoryStream.ToArray();
            }

            return outputStream;
        }
    }
}
