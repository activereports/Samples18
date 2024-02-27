using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GrapeCity.ActiveReports.Document;
using GrapeCity.ActiveReports.Export;
using GrapeCity.ActiveReports.Export.Html;
using GrapeCity.ActiveReports.Export.Html.Section;
using HtmlToOpenXml;
using HtmlToOpenXml.IO;

namespace ActiveReports.Samples.WordExport
{
	public class HtmlToOpenXmlExport : IDocumentExport
	{
		private const string _htmlPath = @"..\..\htmlTemp\";
		private const string _htmlFilePath = _htmlPath + "temp.html";

		private void HtmlToOpenXml(string html, MemoryStream generatedDocument)
		{
			using (var buffer = ResourceHelper.GetStream("Resources.template.docx"))
				buffer.CopyToAsync(generatedDocument);

			generatedDocument.Position = 0L;
			using (var package = WordprocessingDocument.Open(generatedDocument, true))
			{
				MainDocumentPart mainPart = package.MainDocumentPart;
				if (mainPart == null)
				{
					mainPart = package.AddMainDocumentPart();
					new DocumentFormat.OpenXml.Wordprocessing.Document(new Body()).Save(mainPart);
				}

				var converter = new HtmlConverter(mainPart, new CustomWebRequest());
				converter.ParseHtml(html);
				mainPart.Document.Save();
			}
		}

		private void WriteAllBytes(Stream stream, MemoryStream generatedDocument)
		{
			var bytes = generatedDocument.ToArray();
			stream.Write(bytes, 0, bytes.Length);
		}

		private void WriteAllBytes(string destFilePath, MemoryStream generatedDocument)
		{
			File.WriteAllBytes(destFilePath, generatedDocument.ToArray());
		}

		private string HtmlExport(SectionDocument document, string pageRange)
		{
			if (!Directory.Exists(_htmlPath))
			{
				Directory.CreateDirectory(_htmlPath);
			}

			using (var htmlExport = new HtmlExport())
				htmlExport.Export(document, _htmlFilePath, pageRange);
			return GetHtmlString();
		}

		private string GetHtmlString()
		{
			using (var stream = new StreamReader(_htmlFilePath))
			{
				return stream.ReadToEnd();
			}
		}

		private void ClearHtmlPath()
		{
			foreach (var file in new DirectoryInfo(_htmlPath).GetFiles())
			{
				file.Delete();
			}
			Directory.Delete(_htmlPath);
		}

		void IDocumentExport.Export(SectionDocument document, string filePath, string pageRange)
		{
			var html = HtmlExport(document, pageRange);
			using (var generatedDocument = new MemoryStream())
			{
				HtmlToOpenXml(html, generatedDocument);
				WriteAllBytes(filePath, generatedDocument);
			}
			ClearHtmlPath();
		}

		void IDocumentExport.Export(SectionDocument document, string filePath)
		{
			((IDocumentExport)this).Export(document, filePath, string.Empty);
		}

		void IDocumentExport.Export(SectionDocument document, Stream outputStream)
		{
			((IDocumentExport)this).Export(document, outputStream, string.Empty);
		}

		void IDocumentExport.Export(SectionDocument document, Stream outputStream, string pageRange)
		{
			var html = HtmlExport(document, pageRange);
			using (var generatedDocument = new MemoryStream())
			{
				HtmlToOpenXml(html, generatedDocument);
				WriteAllBytes(outputStream, generatedDocument);
			}
			ClearHtmlPath();
		}

		void IDocumentExport.Export(SectionDocument document, IOutputHtml outputHandler, string pageRange)
		{
			throw new NotSupportedException("It's not allowed to use IOutputHtml");
		}

		private class CustomWebRequest : IWebRequest
		{
			public Task<HtmlToOpenXml.IO.Resource> FetchAsync(Uri requestUri, CancellationToken cancellationToken)
			{
				string fileName = Path.GetFileName(requestUri.OriginalString);
				var imagePath = Path.Combine(_htmlPath, fileName);

				var task = new Task<HtmlToOpenXml.IO.Resource>(() =>
				{
					var res = new HtmlToOpenXml.IO.Resource();
					res.Content = File.OpenRead(imagePath);
					return res;
				});
				task.Start();

				return task;
			}

			public void Dispose()
			{
				throw new NotImplementedException();
			}

			public bool SupportsProtocol(string protocol)
			{
				throw new NotImplementedException();
			}
		}
	}    
}
