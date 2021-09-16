using System.ComponentModel.DataAnnotations;

namespace Dii_OrderingSvc.Fake.Data
{
    public enum ThemeType
    {
        [Display(Description = "A calm blue sky")]
        Cerulean = 1,
        [Display(Description = "An ode to Metro")]
        Cosmo = 2,
        [Display(Description = "Jet black and electric blue")]
        Cyborg = 3,
        [Display(Description = "Flatly in night mode")]
        Darkly = 4,
        [Display(Description = "Flat and modern")]
        Flatly = 5,
        [Display(Description = "Crisp like a new sheet of paper")]
        Journal = 6,
        [Display(Description = "The medium is the message")]
        Litera = 7,
        [Display(Description = "Light and shadow")]
        Lumen = 8,
        [Display(Description = "A touch of class")]
        Lux = 9,
        [Display(Description = "Material is the metaphor")]
        Materia = 10,
        [Display(Description = "A fresh feel")]
        Minty = 11,
        [Display(Description = "A trace of purple")]
        Pulse = 12,
        [Display(Description = "A touch of warmth")]
        Sandstone = 13,
        [Display(Description = "Mini and minimalist")]
        Simplex = 14,
        [Display(Description = "A hand-drawn look for mockups and mirth")]
        Sketchy = 15,
        [Display(Description = "Shades of gunmetal gray")]
        Slate = 16,
        [Display(Description = "A spin on Solarized")]
        Solar = 17,
        [Display(Description = "Silvery and sleek")]
        Spacelab = 18,
        [Display(Description = "The brave and the blue")]
        Superhero = 19,
        [Display(Description = "Ubuntu orange and unique font")]
        United = 20,
        [Display(Description = "A friendly foundation")]
        Yeti = 21
    }
}
