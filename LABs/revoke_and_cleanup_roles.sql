-- ==============================================================
-- СКРИПТ: Полная очистка ролей и пользователей
-- Цель: Удалить все созданные роли и пользователей
--       Чтобы можно было заново выполнить setup_roles_and_permissions.sql
-- ==============================================================

-- 1. Отключаем все активные соединения с базой (кроме текущего)
-- ВАЖНО: Запускай от суперпользователя (postgres)
SELECT pg_terminate_backend(pg_stat_activity.pid)
FROM pg_stat_activity
WHERE pg_stat_activity.datname = current_database()
  AND pid <> pg_backend_pid()
  AND usename IN ('user_admin', 'user_manager', 'user_employee', 'user_accountant');

-- 2. Отзываем все права у ролей
REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM 
    role_admin, role_warehouse_manager, role_warehouse_employee, role_accountant;

REVOKE ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public FROM 
    role_admin, role_warehouse_manager, role_warehouse_employee, role_accountant;

REVOKE USAGE ON SCHEMA public FROM 
    role_admin, role_warehouse_manager, role_warehouse_employee, role_accountant;

-- 3. Удаляем пользователей (если они существуют)
DO $$
BEGIN
    -- Удаляем пользователей (они могут быть владельцами объектов — сначала удаляем связи)
    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_admin') THEN
        DROP OWNED BY user_admin CASCADE;
        DROP ROLE user_admin;
    END IF;

    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_manager') THEN
        DROP OWNED BY user_manager CASCADE;
        DROP ROLE user_manager;
    END IF;

    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_employee') THEN
        DROP OWNED BY user_employee CASCADE;
        DROP ROLE user_employee;
    END IF;

    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'user_accountant') THEN
        DROP OWNED BY user_accountant CASCADE;
        DROP ROLE user_accountant;
    END IF;
END $$;

-- 4. Удаляем групповые роли
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_admin') THEN
        DROP OWNED BY role_admin CASCADE;
        DROP ROLE role_admin;
    END IF;

    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_warehouse_manager') THEN
        DROP OWNED BY role_warehouse_manager CASCADE;
        DROP ROLE role_warehouse_manager;
    END IF;

    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_warehouse_employee') THEN
        DROP OWNED BY role_warehouse_employee CASCADE;
        DROP ROLE role_warehouse_employee;
    END IF;

    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'role_accountant') THEN
        DROP OWNED BY role_accountant CASCADE;
        DROP ROLE role_accountant;
    END IF;
END $$;

-- ==============================================================
-- ГОТОВО! Теперь можно заново запустить:
--   psql -U postgres -d ORBD -f setup_roles_and_permissions.sql
-- ==============================================================