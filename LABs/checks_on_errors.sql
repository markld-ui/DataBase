-- Подготовка: Добавим минимальные корректные данные для проверки FK
INSERT INTO warehouse (name, address) VALUES
('Тестовый склад', 'г. Тест, ул. Тестовая, 1');

INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(1, 1000, 'refrigerated', 'Тестовая зона');

INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES
('Тестовый товар', '2025-12-01', 'food', TRUE, NULL);

INSERT INTO supplier (company_name, contact_person, phone, address) VALUES
('Тестовая компания', 'Тест Тестов', '+79999999999', 'г. Тест, ул. Тестовая, 2');

INSERT INTO employee (photo, full_name, position, phone) VALUES
(NULL, 'Тестов Тест Тестович', 'Тестер', '+79999999998');

INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(1, 1, '2025-04-01', 100);

-- Тесты ограничений для таблицы warehouse
-- 1. Проверка NOT NULL для name
INSERT INTO warehouse (name, address) VALUES
(NULL, 'г. Тест, ул. Тестовая, 3');
-- Ожидаемая ошибка: column "name" of relation "warehouse" does not allow null values

-- 2. Проверка NOT NULL для address
INSERT INTO warehouse (name, address) VALUES
('Склад 2', NULL);
-- Ожидаемая ошибка: column "address" of relation "warehouse" does not allow null values

-- 3. Проверка уникальности PK (warehouse_id)
-- (SERIAL не позволяет вручную задать дубликат, но попробуем)
INSERT INTO warehouse (warehouse_id, name, address) VALUES
(1, 'Дубликат', 'г. Тест, ул. Тестовая, 4');
-- Ожидаемая ошибка: duplicate key value violates unique constraint "warehouse_pkey"

-- Тесты ограничений для таблицы storage_zone
-- 4. Проверка NOT NULL для warehouse_id
INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(NULL, 500, 'dry', 'Зона без склада');
-- Ожидаемая ошибка: column "warehouse_id" of relation "storage_zone" does not allow null values

-- 5. Проверка FK для warehouse_id
INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(999, 500, 'dry', 'Неправильный склад');
-- Ожидаемая ошибка: insert or update on table "storage_zone" violates foreign key constraint "fk_warehouse"

-- 6. Проверка CHECK для capacity (>= 0)
INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(1, -100, 'dry', 'Отрицательная вместимость');
-- Ожидаемая ошибка: new row for relation "storage_zone" violates check constraint "storage_zone_capacity_check"

-- 7. Проверка NOT NULL для zone_type
INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(1, 500, NULL, 'Зона без типа');
-- Ожидаемая ошибка: column "zone_type" of relation "storage_zone" does not allow null values

-- 8. Проверка ENUM для zone_type
INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(1, 500, 'invalid_type', 'Неправильный тип');
-- Ожидаемая ошибка: invalid input value for enum zone_type

-- 9. Проверка NOT NULL для zone_name
INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(1, 500, 'dry', NULL);
-- Ожидаемая ошибка: column "zone_name" of relation "storage_zone" does not allow null values

-- Тесты ограничений для таблицы product
-- 10. Проверка NOT NULL для name
INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES
(NULL, '2025-12-01', 'food', TRUE, NULL);
-- Ожидаемая ошибка: column "name" of relation "product" does not allow null values

-- 11. Проверка NOT NULL для product_type
INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES
('Товар без типа', '2025-12-01', NULL, TRUE, NULL);
-- Ожидаемая ошибка: column "product_type" of relation "product" does not allow null values

-- 12. Проверка ENUM для product_type
INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES
('Неправильный товар', '2025-12-01', 'invalid_type', TRUE, NULL);
-- Ожидаемая ошибка: invalid input value for enum product_type

-- 13. Проверка NOT NULL для is_active
INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES
('Товар без активности', '2025-12-01', 'food', NULL, NULL);
-- Ожидаемая ошибка: column "is_active" of relation "product" does not allow null values

-- 14. Проверка типа BOOLEAN для is_active
-- PostgreSQL автоматически преобразует 0/1 в FALSE/TRUE, но попробуем некорректный тип
-- (Прямой тест сложен, так как PostgreSQL строг к BOOLEAN, но проверим логику)
INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES
('Товар с некорректным BOOLEAN', '2025-12-01', 'food', 'not_a_boolean', NULL);
-- Ожидаемая ошибка: column "is_active" is of type boolean but expression is of type text

-- Тесты ограничений для таблицы supplier
-- 15. Проверка NOT NULL для company_name
INSERT INTO supplier (company_name, contact_person, phone, address) VALUES
(NULL, 'Тест Тестов', '+79999999997', 'г. Тест, ул. Тестовая, 5');
-- Ожидаемая ошибка: column "company_name" of relation "supplier" does not allow null values

-- Тесты ограничений для таблицы employee
-- 16. Проверка NOT NULL для full_name
INSERT INTO employee (photo, full_name, position, phone) VALUES
(NULL, NULL, 'Тестер', '+79999999996');
-- Ожидаемая ошибка: column "full_name" of relation "employee" does not allow null values

-- 17. Проверка NOT NULL для position
INSERT INTO employee (photo, full_name, position, phone) VALUES
(NULL, 'Тестов Тест Тестович', NULL, '+79999999995');
-- Ожидаемая ошибка: column "position" of relation "employee" does not allow null values

-- Тесты ограничений для таблицы supply
-- 18. Проверка NOT NULL для product_id
INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(NULL, 1, '2025-04-02', 200);
-- Ожидаемая ошибка: column "product_id" of relation "supply" does not allow null values

-- 19. Проверка FK для product_id
INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(999, 1, '2025-04-02', 200);
-- Ожидаемая ошибка: insert or update on table "supply" violates foreign key constraint "fk_product"

-- 20. Проверка NOT NULL для supplier_id
INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(1, NULL, '2025-04-02', 200);
-- Ожидаемая ошибка: column "supplier_id" of relation "supply" does not allow null values

-- 21. Проверка FK для supplier_id
INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(1, 999, '2025-04-02', 200);
-- Ожидаемая ошибка: insert or update on table "supply" violates foreign key constraint "fk_supplier"

-- 22. Проверка NOT NULL для supply_date
INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(1, 1, NULL, 200);
-- Ожидаемая ошибка: column "supply_date" of relation "supply" does not allow null values

-- 23. Проверка CHECK для quantity (> 0)
INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(1, 1, '2025-04-02', 0);
-- Ожидаемая ошибка: new row for relation "supply" violates check constraint "supply_quantity_check"

-- Тесты ограничений для таблицы product_accounting
-- 24. Проверка NOT NULL для supply_id
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(NULL, 1, 1, '2025-04-02', 100, '2025-04-03');
-- Ожидаемая ошибка: column "supply_id" of relation "product_accounting" does not allow null values

-- 25. Проверка FK для supply_id
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(999, 1, 1, '2025-04-02', 100, '2025-04-03');
-- Ожидаемая ошибка: insert or update on table "product_accounting" violates foreign key constraint "fk_supply"

-- 26. Проверка NOT NULL для employee_id
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(1, NULL, 1, '2025-04-02', 100, '2025-04-03');
-- Ожидаемая ошибка: column "employee_id" of relation "product_accounting" does not allow null values

-- 27. Проверка FK для employee_id
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(1, 999, 1, '2025-04-02', 100, '2025-04-03');
-- Ожидаемая ошибка: insert or update on table "product_accounting" violates foreign key constraint "fk_employee"

-- 28. Проверка NOT NULL для storage_id
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(1, 1, NULL, '2025-04-02', 100, '2025-04-03');
-- Ожидаемая ошибка: column "storage_id" of relation "product_accounting" does not allow null values

-- 29. Проверка FK для storage_id
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(1, 1, 999, '2025-04-02', 100, '2025-04-03');
-- Ожидаемая ошибка: insert or update on table "product_accounting" violates foreign key constraint "fk_storage"

-- 30. Проверка NOT NULL для accounting_date
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(1, 1, 1, NULL, 100, '2025-04-03');
-- Ожидаемая ошибка: column "accounting_date" of relation "product_accounting" does not allow null values

-- 31. Проверка CHECK для quantity (>= 0)
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(1, 1, 1, '2025-04-02', -100, '2025-04-03');
-- Ожидаемая ошибка: new row for relation "product_accounting" violates check constraint "product_accounting_quantity_check"