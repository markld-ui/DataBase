-- ==============================================================
-- 1. УСТАНОВКА СХЕМЫ (если не в public)
-- ==============================================================
-- Если таблицы созданы в public — оставляем как есть.
-- Если в отдельной схеме (например, warehouse_db), замените:
-- SET search_path TO warehouse_db;
SET search_path TO public;

-- ==============================================================
-- 2. СОЗДАНИЕ РОЛЕЙ (группы)
-- ==============================================================
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_admin') THEN
        CREATE ROLE role_admin;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_warehouse_manager') THEN
        CREATE ROLE role_warehouse_manager;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_warehouse_employee') THEN
        CREATE ROLE role_warehouse_employee;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_accountant') THEN
        CREATE ROLE role_accountant;
    END IF;
END $$;

-- ==============================================================
-- 3. СОЗДАНИЕ ПОЛЬЗОВАТЕЛЕЙ (с входом и паролями)
-- ==============================================================
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_admin') THEN
        CREATE USER user_admin PASSWORD 'admin123' IN ROLE role_admin;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_manager') THEN
        CREATE USER user_manager PASSWORD 'manager123' IN ROLE role_warehouse_manager;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_employee') THEN
        CREATE USER user_employee PASSWORD 'employee123' IN ROLE role_warehouse_employee;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_accountant') THEN
        CREATE USER user_accountant PASSWORD 'accountant123' IN ROLE role_accountant;
    END IF;
END $$;

-- ==============================================================
-- 4. БАЗОВЫЕ ПРАВА: USAGE на схему + SEQUENCE
-- ==============================================================
GRANT USAGE ON SCHEMA public TO role_admin, role_warehouse_manager, role_warehouse_employee, role_accountant;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO role_admin, role_warehouse_manager, role_warehouse_employee, role_accountant;

-- ==============================================================
-- 5. ПОЛНЫЕ ПРАВА ДЛЯ АДМИНА
-- ==============================================================
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO role_admin;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO role_admin;

-- ==============================================================
-- 6. РАЗГРАНИЧЕНИЕ ПРАВ ПО РОЛЯМ
-- ==============================================================

-- ------------------- МЕНЕДЖЕР СКЛАДА (role_warehouse_manager) -------------------
GRANT SELECT, INSERT, UPDATE ON product TO role_warehouse_manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON supply TO role_warehouse_manager;
GRANT SELECT, INSERT, UPDATE ON supplier TO role_warehouse_manager;
GRANT SELECT ON product_accounting TO role_warehouse_manager;
GRANT SELECT ON employee TO role_warehouse_manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON storage_zone TO role_warehouse_manager;
GRANT SELECT, INSERT, UPDATE ON warehouse TO role_warehouse_manager;

-- ------------------- СОТРУДНИК СКЛАДА (role_warehouse_employee) ----------------
GRANT SELECT ON product TO role_warehouse_employee;
GRANT SELECT ON supply TO role_warehouse_employee;
GRANT SELECT ON supplier TO role_warehouse_employee;
GRANT SELECT, INSERT, UPDATE ON product_accounting TO role_warehouse_employee; -- перемещение, приёмка
GRANT SELECT ON employee TO role_warehouse_employee;
GRANT SELECT ON storage_zone TO role_warehouse_employee;
GRANT SELECT ON warehouse TO role_warehouse_employee;

-- ------------------- БУХГАЛТЕР (role_accountant) -------------------------------
GRANT SELECT ON product TO role_accountant;
GRANT SELECT, INSERT ON supply TO role_accountant;                            -- учёт поставок
GRANT SELECT ON supplier TO role_accountant;
GRANT SELECT, INSERT, UPDATE, DELETE ON product_accounting TO role_accountant; -- полный учёт
GRANT SELECT ON employee TO role_accountant;
GRANT SELECT ON storage_zone TO role_accountant;
GRANT SELECT ON warehouse TO role_accountant;