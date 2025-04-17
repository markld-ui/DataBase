-- Очистка таблиц (если нужно)
TRUNCATE TABLE product_accounting, supply, employee, storage_zone, warehouse, product, supplier RESTART IDENTITY CASCADE;

-- Заполнение warehouse
INSERT INTO warehouse (name, address) VALUES
('Центральный склад', 'г. Москва, ул. Ленина, 10'),
('Северный склад', 'г. Санкт-Петербург, ул. Мира, 5'),
('Южный склад', 'г. Ростов-на-Дону, ул. Солнечная, 20'),
('Восточный склад', 'г. Новосибирск, ул. Центральная, 15');

-- Заполнение storage_zone
INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES
(1, 1000, 'refrigerated', 'Холодильная зона 1'),
(1, 2000, 'dry', 'Сухая зона 1'),
(2, 1500, 'frozen', 'Морозильная зона 1'),
(3, 3000, 'general', 'Общая зона 1'),
(4, 1200, 'refrigerated', 'Холодильная зона 2');

-- Заполнение product
INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES
('Молоко', '2025-05-01', 'food', TRUE, NULL),
('Смартфон', NULL, 'electronics', TRUE, NULL),
('Футболка', NULL, 'clothing', TRUE, NULL),
('Книга', NULL, 'other', FALSE, NULL),
('Яблоки', '2025-06-01', 'food', TRUE, NULL);

-- Заполнение supplier
INSERT INTO supplier (company_name, contact_person, phone, address) VALUES
('Молочная ферма', 'Иванов Иван', '+79991234567', 'г. Москва, ул. Полянка, 12'),
('ТехноТрейд', 'Петров Петр', '+79992345678', 'г. Санкт-Петербург, ул. Невская, 8'),
('Модный мир', 'Сидорова Анна', '+79993456789', 'г. Ростов-на-Дону, ул. Весенняя, 3'),
('Книжный дом', 'Козлов Михаил', '+79994567890', 'г. Новосибирск, ул. Книжная, 7');

-- Заполнение employee
INSERT INTO employee (photo, full_name, position, phone) VALUES
(NULL, 'Смирнова Елена', 'Кладовщик', '+79995678901'),
(NULL, 'Кузнецов Алексей', 'Менеджер', '+79996789012'),
(NULL, 'Васильева Ольга', 'Кладовщик', '+79997890123'),
(NULL, 'Морозов Дмитрий', 'Логист', '+79998901234'),
(NULL, 'Лебедева Мария', 'Кладовщик', '+79999012345');

-- Заполнение supply
INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES
(1, 1, '2025-04-01', 1000),
(2, 2, '2025-04-02', 500),
(3, 3, '2025-04-03', 200),
(4, 4, '2025-04-04', 300),
(5, 1, '2025-04-05', 1500);

-- Заполнение product_accounting
INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date) VALUES
(1, 1, 1, '2025-04-01', 1000, '2025-04-02'),
(2, 2, 2, '2025-04-02', 500, '2025-04-03'),
(3, 3, 3, '2025-04-03', 200, '2025-04-04'),
(4, 4, 4, '2025-04-04', 300, '2025-04-05'),
(5, 5, 5, '2025-04-05', 1500, '2025-04-06');