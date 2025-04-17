-- Создание типа ENUM для дискретных значений
CREATE TYPE product_type AS ENUM ('food', 'electronics', 'clothing', 'other');
CREATE TYPE zone_type AS ENUM ('refrigerated', 'dry', 'frozen', 'general');

-- Таблица "Склад"
CREATE TABLE warehouse (
    warehouse_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    address VARCHAR(200) NOT NULL,
    CONSTRAINT warehouse_id_positive CHECK (warehouse_id > 0)
);

-- Таблица "Зона хранения"
CREATE TABLE storage_zone (
    storage_id SERIAL PRIMARY KEY,
    warehouse_id INT NOT NULL,
    capacity INT NOT NULL CHECK (capacity >= 0),
    zone_type zone_type NOT NULL,
    zone_name VARCHAR(100) NOT NULL,
    CONSTRAINT storage_id_positive CHECK (storage_id > 0),
    CONSTRAINT fk_warehouse FOREIGN KEY (warehouse_id) REFERENCES warehouse(warehouse_id) ON DELETE RESTRICT
);

-- Таблица "Товар"
CREATE TABLE product (
    product_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    expiry_date DATE,
    product_type product_type NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    photo BYTEA,
    CONSTRAINT product_id_positive CHECK (product_id > 0)
);

-- Таблица "Поставщик"
CREATE TABLE supplier (
    supplier_id SERIAL PRIMARY KEY,
    company_name VARCHAR(100) NOT NULL,
    contact_person VARCHAR(100),
    phone VARCHAR(20),
    address VARCHAR(200),
    CONSTRAINT supplier_id_positive CHECK (supplier_id > 0)
);

-- Таблица "Персонал"
CREATE TABLE employee (
    employee_id SERIAL PRIMARY KEY,
    photo BYTEA,
    full_name VARCHAR(100) NOT NULL,
    position VARCHAR(50) NOT NULL,
    phone VARCHAR(20),
    CONSTRAINT employee_id_positive CHECK (employee_id > 0)
);

-- Таблица "Поставка"
CREATE TABLE supply (
    supply_id SERIAL PRIMARY KEY,
    product_id INT NOT NULL,
    supplier_id INT NOT NULL,
    supply_date DATE NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    CONSTRAINT supply_id_positive CHECK (supply_id > 0),
    CONSTRAINT fk_product FOREIGN KEY (product_id) REFERENCES product(product_id) ON DELETE RESTRICT,
    CONSTRAINT fk_supplier FOREIGN KEY (supplier_id) REFERENCES supplier(supplier_id) ON DELETE RESTRICT
);

-- Таблица "Учет товара"
CREATE TABLE product_accounting (
    productAcc_id SERIAL PRIMARY KEY,
    supply_id INT NOT NULL,
    employee_id INT NOT NULL,
    storage_id INT NOT NULL,
    accounting_date DATE NOT NULL,
    quantity INT NOT NULL CHECK (quantity >= 0),
    last_movement_date DATE,
    CONSTRAINT productAcc_id_positive CHECK (productAcc_id > 0),
    CONSTRAINT fk_supply FOREIGN KEY (supply_id) REFERENCES supply(supply_id) ON DELETE RESTRICT,
    CONSTRAINT fk_employee FOREIGN KEY (employee_id) REFERENCES employee(employee_id) ON DELETE RESTRICT,
    CONSTRAINT fk_storage FOREIGN KEY (storage_id) REFERENCES storage_zone(storage_id) ON DELETE RESTRICT
);