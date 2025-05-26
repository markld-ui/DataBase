using System;
using System.Windows.Forms;
using UI;
using Xunit;

namespace Tests
{
    public class WarehouseFormTests : IDisposable
    {
        private readonly WarehouseForm _form;

        public WarehouseFormTests()
        {
            _form = new WarehouseForm();
        }

        [Fact]
        public void ValidateInput_EmptyName_ReturnsFalse()
        {
            // Arrange
            _form.Controls["txtName"].Text = "";

            // Act
            var result = _form.GetType().GetMethod("ValidateInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(_form, null);

            // Assert
            Assert.False((bool)result);
        }

        [Fact]
        public void ValidateInput_ValidName_ReturnsTrue()
        {
            // Arrange
            _form.Controls["txtName"].Text = "Склад 1";

            // Act
            var result = _form.GetType().GetMethod("ValidateInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(_form, null);

            // Assert
            Assert.True((bool)result);
        }

        public void Dispose()
        {
            _form.Dispose();
        }
    }
}