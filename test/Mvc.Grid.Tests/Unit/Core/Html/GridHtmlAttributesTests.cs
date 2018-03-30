using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridHtmlAttributesTests
    {
        #region GridHtmlAttributes()

        [Fact]
        public void GridHtmlAttributes_Empty()
        {
            Assert.Empty(new GridHtmlAttributes().ToHtmlString());
        }

        #endregion

        #region GridHtmlAttributes(Object htmlAttributes)

        [Fact]
        public void GridHtmlAttributes_ChangesUnderscoresToDashes()
        {
            String actual = new GridHtmlAttributes(new { id = "", data_null = (String)null, data_temp = 10000, src = "test.png" }).ToHtmlString();
            String expected = " id=\"\" data-null=\"\" data-temp=\"10000\" src=\"test.png\"";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region WriteTo(TextWriter writer, HtmlEncoder encoder)

        [Fact]
        public void WriteTo_EncodesValues()
        {
            String actual = new GridHtmlAttributes(new { value = "Temp \"str\"" }).ToHtmlString();
            String expected = " value=\"Temp &quot;str&quot;\"";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
