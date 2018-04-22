namespace Mailer.Model
{
    public class ColorScheme
    {
        public ColorScheme(string name, string color)
        {
            Name = name;
            Color = color;
        }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}