namespace ManagerDish.Helpers
{
    public class FileHelper
    {
        public static async Task<string> SaveImageAsync(IFormFile file, string folderName)
        {
            if(file == null || file.Length == 0)
            {
                return "/images/avartars/default-avatar.png";
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/images/{folderName}/{fileName}";
        }
    }
}
