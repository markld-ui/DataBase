using Moq;
using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Models;
using Xunit;

namespace Tests
{
    public class WarehouseRepositoryTests
    {
        private readonly Mock<IWarehouseRepository> _warehouseRepositoryMock;
        private readonly List<Warehouse> _warehouses;

        public WarehouseRepositoryTests()
        {
            _warehouseRepositoryMock = new Mock<IWarehouseRepository>();

            // Подготовка тестовых данных
            _warehouses = new List<Warehouse>
            {
                new Warehouse { WarehouseId = 1, Name = "Склад 1", Address = "Адрес 1" },
                new Warehouse { WarehouseId = 2, Name = "Склад 2", Address = "Адрес 2" }
            };

            // Настройка поведения мок-объекта
            _warehouseRepositoryMock.Setup(repo => repo.GetAll()).Returns(_warehouses);
            _warehouseRepositoryMock.Setup(repo => repo.Add(It.IsAny<Warehouse>())).Callback<Warehouse>(warehouse =>
            {
                warehouse.WarehouseId = _warehouses.Count + 1;
                _warehouses.Add(warehouse);
            });
            _warehouseRepositoryMock.Setup(repo => repo.Update(It.IsAny<Warehouse>())).Callback<Warehouse>(warehouse =>
            {
                var existing = _warehouses.Find(w => w.WarehouseId == warehouse.WarehouseId);
                if (existing != null)
                {
                    existing.Name = warehouse.Name;
                    existing.Address = warehouse.Address;
                }
            });
            _warehouseRepositoryMock.Setup(repo => repo.Delete(It.IsAny<int>())).Callback<int>(id =>
            {
                var warehouse = _warehouses.Find(w => w.WarehouseId == id);
                if (warehouse != null)
                {
                    _warehouses.Remove(warehouse);
                }
            });
        }

        [Fact]
        public void GetAll_ReturnsAllWarehouses()
        {
            // Act
            var result = _warehouseRepositoryMock.Object.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Склад 1", result[0].Name);
            Assert.Equal("Склад 2", result[1].Name);
        }

        [Fact]
        public void Add_IncreasesWarehouseCount()
        {
            // Arrange
            var newWarehouse = new Warehouse { Name = "Склад 3", Address = "Адрес 3" };

            // Act
            _warehouseRepositoryMock.Object.Add(newWarehouse);

            // Assert
            Assert.Equal(3, _warehouses.Count);
            Assert.Equal("Склад 3", _warehouses[2].Name);
            Assert.Equal(3, _warehouses[2].WarehouseId);
        }

        [Fact]
        public void Update_UpdatesExistingWarehouse()
        {
            // Arrange
            var updatedWarehouse = new Warehouse { WarehouseId = 1, Name = "Обновлённый склад", Address = "Новый адрес" };

            // Act
            _warehouseRepositoryMock.Object.Update(updatedWarehouse);

            // Assert
            var result = _warehouses.Find(w => w.WarehouseId == 1);
            Assert.NotNull(result);
            Assert.Equal("Обновлённый склад", result.Name);
            Assert.Equal("Новый адрес", result.Address);
        }

        [Fact]
        public void Delete_RemovesWarehouse()
        {
            // Act
            _warehouseRepositoryMock.Object.Delete(1);

            // Assert
            Assert.Equal(1, _warehouses.Count);
            Assert.Null(_warehouses.Find(w => w.WarehouseId == 1));
        }
    }
}