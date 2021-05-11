using QuartierLatin.Backend.Utils;
using QuartierLatin.Importer;
using QuartierLatin.Importer.DataModel;
using Xunit;

namespace QuartierLatin.Backend.Tests.Utils
{
    public class UrlizerTests
    {
        [Theory,
        InlineData("университет", "universitet"),
            InlineData("Academy of sciences", "academy-of-sciences"),
            InlineData("École Supérieure Libre des Sciences Commerciales Appliquées (ESLSCA)", "ecole-superieure-libre-des-sciences-commerciales-appliquees-eslsca")
        
        ]
        public void ShouldUrlize(string original, string url)
        {
            Assert.Equal(url, Urlizer.Urlize(original));
        }
    }
}