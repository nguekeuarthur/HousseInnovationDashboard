using System.Diagnostics;
using HousseInnovation.DAL;
using HousseInnovation.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace HousseInnovation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _web;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDBContext db, IWebHostEnvironment web)
        {
            _logger = logger;
            _db = db;
            _web = web;
        }

        [BindProperty]
        public Client client { get; set; }
        [BindProperty]
        public List<ClientViewModel> ClientList { get; set; }
        [BindProperty]
        public int NombreApplication { get; set; }
        [BindProperty]
        public int NombreClient { get; set; }

        public async Task OnGet()
        {
            var groupedClients = _db.Client
                .Where(x => x.IsExist == true)
                .GroupBy(x => x.ClientName)
                .Select(group => new ClientViewModel
                {
                    ClientName = group.Key,
                    Applications = group.Select(x => x.AppName).ToList()
                })
                .ToList();

            ClientList = groupedClients;
            NombreApplication = groupedClients.Sum(x => x.Applications.Count);
            NombreClient = groupedClients.Count();
        }

        public async Task OnPostAsync()
        {
            client.IsExist = true;
            _db.Add(client);
            _db.SaveChanges();

            string rootPath = _web.ContentRootPath;
            string clientDir = client.ClientName;
            string appDir = client.AppName;
            string firstDirPath = Path.Combine(rootPath, clientDir);
            if (!Directory.Exists(firstDirPath))
            {
                Directory.CreateDirectory(firstDirPath);
            }
            string secondDirPath = Path.Combine(firstDirPath, appDir);
            if (!Directory.Exists(secondDirPath))
            {
                Directory.CreateDirectory(secondDirPath);
            }

            await OnFichierAsync(secondDirPath);

            string currentDirPath = Directory.GetCurrentDirectory();
            string[] csvFiles = Directory.GetFiles(currentDirPath, "*.csv", SearchOption.TopDirectoryOnly);

            foreach (var csvFile in csvFiles)
            {
                string destinationPath = Path.Combine(secondDirPath, Path.GetFileName(csvFile));

                if (System.IO.File.Exists(destinationPath))
                {
                    System.IO.File.Delete(destinationPath);
                }

                System.IO.File.Move(csvFile, destinationPath);
            }

            await LaunchStreamlitAppAsync(secondDirPath);

            await OnGet();
        }

        public async Task<IActionResult> OnFichierAsync(string targetDirectory)
        {
            string sourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Controllers");

            try
            {
                DirectoryCopy(sourceDirectory, targetDirectory, true);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return Page();
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private async Task LaunchStreamlitAppAsync(string appDirectory)
        {
            string appPath = "app.py";
            var appStartInfo = new ProcessStartInfo
            {
                FileName = "streamlit",
                Arguments = $"run {appPath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = appDirectory
            };

            try
            {
                using (var appProcess = Process.Start(appStartInfo))
                {
                    if (appProcess != null)
                    {
                        string appOutput = await appProcess.StandardOutput.ReadToEndAsync();
                        string appError = await appProcess.StandardError.ReadToEndAsync();

                        appProcess.WaitForExit();

                        if (!string.IsNullOrEmpty(appError))
                        {
                            ModelState.AddModelError(string.Empty, appError);
                        }
                        else
                        {
                            ViewData["AppOutput"] = appOutput;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
    }

    public class ClientViewModel
    {
        public string ClientName { get; set; }
        public List<string> Applications { get; set; }
    }
}
