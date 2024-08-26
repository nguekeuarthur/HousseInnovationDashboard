//using System.Diagnostics;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//public class DashboardController : Controller
//{
//	public IActionResult Index()
//	{
//		return View();
//	}

//	[HttpPost]
//	public async Task<IActionResult> RegisterClient(string ClientName, string AppName, IFormFile JsonFile)
//	{
//		if (JsonFile != null && JsonFile.Length > 0)
//		{
//			// Créez le chemin du dossier Client
//			var clientDirectory = Path.Combine("wwwroot", "clients", ClientName);

//			// Créez le chemin du sous-dossier Application
//			var appDirectory = Path.Combine(clientDirectory, AppName);

//			// Créez les dossiers s'ils n'existent pas
//			if (!Directory.Exists(clientDirectory))
//			{
//				Directory.CreateDirectory(clientDirectory);
//			}

//			if (!Directory.Exists(appDirectory))
//			{
//				Directory.CreateDirectory(appDirectory);
//			}

//			// Chemin complet pour enregistrer le fichier JSON
//			var jsonFilePath = Path.Combine(appDirectory, "myDhoola.json");

//			// Enregistrez le fichier JSON
//			using (var stream = new FileStream(jsonFilePath, FileMode.Create))
//			{
//				await JsonFile.CopyToAsync(stream);
//			}

//			// Exécuter les scripts Python
//			ExecuteScripts();

//			// Redirigez vers le tableau de bord ou une autre page
//			return RedirectToAction("Index");
//		}

//		// Si le fichier JSON n'est pas fourni, renvoyez une erreur ou restez sur la page actuelle
//		return View("Index");
//	}

//	public IActionResult ExecuteScripts()
//	{
//		// Chemin des scripts Python
//		string exportScriptPath = "exportCSV.py";
//		string appScriptPath = "app.py";

//		// Exécuter exportCSV.py 
//		ProcessStartInfo exportStartInfo = new ProcessStartInfo
//		{
//			FileName = "python",
//			Arguments = exportScriptPath,
//			RedirectStandardOutput = true,
//			RedirectStandardError = true,
//			UseShellExecute = false,
//			CreateNoWindow = true
//		};

//		using (Process exportProcess = new Process())
//		{
//			exportProcess.StartInfo = exportStartInfo;
//			exportProcess.Start();
//			exportProcess.WaitForExit();
//		}

//		// Exécuter App.py
//		ProcessStartInfo appStartInfo = new ProcessStartInfo
//		{
//			FileName = "python",
//			Arguments = appScriptPath,
//			RedirectStandardOutput = true,
//			RedirectStandardError = true,
//			UseShellExecute = false,
//			CreateNoWindow = true
//		};

//		using (Process appProcess = new Process())
//		{
//			appProcess.StartInfo = appStartInfo;
//			appProcess.Start();
//			appProcess.WaitForExit();
//		}

//		return RedirectToAction("Index");
//	}
//}
