-- Удаление существующих таблиц, если они есть
DROP TABLE IF EXISTS product_accounting CASCADE;
DROP TABLE IF EXISTS supply CASCADE;
DROP TABLE IF EXISTS storage_zone CASCADE;
DROP TABLE IF EXISTS warehouse CASCADE;
DROP TABLE IF EXISTS product CASCADE;
DROP TABLE IF EXISTS supplier CASCADE;
DROP TABLE IF EXISTS employee CASCADE;
DROP TYPE IF EXISTS product_type CASCADE;
DROP TYPE IF EXISTS zone_type CASCADE;

-- Создание перечислений
CREATE TYPE product_type AS ENUM ('food', 'electronics', 'clothing', 'other');
CREATE TYPE zone_type AS ENUM ('refrigerated', 'dry', 'frozen', 'general');

-- Создание таблицы warehouse (Склад)
CREATE TABLE warehouse (
    warehouse_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    address TEXT NOT NULL
);

-- Создание таблицы storage_zone (Зона хранения)
CREATE TABLE storage_zone (
    storage_id SERIAL PRIMARY KEY,
    warehouse_id INTEGER NOT NULL,
    capacity INTEGER NOT NULL CHECK (capacity >= 0),
    zone_type zone_type NOT NULL,
    zone_name VARCHAR(255) NOT NULL,
    FOREIGN KEY (warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Создание таблицы product (Товар)
CREATE TABLE product (
    product_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    expiry_date DATE,
    product_type product_type NOT NULL,
    is_active BOOLEAN NOT NULL,
    photo BYTEA
);

-- Создание таблицы supplier (Поставщик)
CREATE TABLE supplier (
    supplier_id SERIAL PRIMARY KEY,
    company_name VARCHAR(255) NOT NULL,
    contact_person VARCHAR(255),
    phone VARCHAR(20),
    address TEXT
);

-- Создание таблицы employee (Персонал)
CREATE TABLE employee (
    employee_id SERIAL PRIMARY KEY,
    photo BYTEA,
    full_name VARCHAR(255) NOT NULL,
    position VARCHAR(100) NOT NULL,
    phone VARCHAR(20)
);

-- Создание таблицы supply (Поставка)
CREATE TABLE supply (
    supply_id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL,
    supplier_id INTEGER NOT NULL,
    supply_date DATE NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    FOREIGN KEY (product_id) REFERENCES product(product_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (supplier_id) REFERENCES supplier(supplier_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Создание таблицы product_accounting (Учет продукции)
CREATE TABLE product_accounting (
    productAcc_id SERIAL PRIMARY KEY,
    supply_id INTEGER NOT NULL,
    employee_id INTEGER NOT NULL,
    storage_id INTEGER NOT NULL,
    accounting_date DATE NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity >= 0),
    last_movement_date DATE,
    FOREIGN KEY (supply_id) REFERENCES supply(supply_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (employee_id) REFERENCES employee(employee_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (storage_id) REFERENCES storage_zone(storage_id) ON DELETE CASCADE ON UPDATE CASCADE
);