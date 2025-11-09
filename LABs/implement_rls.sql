-- 1. Удаляем старые политики и функцию
DROP POLICY IF EXISTS emp_read_policy ON employee;
DROP POLICY IF EXISTS acc_read_policy ON product_accounting;
DROP POLICY IF EXISTS acc_insert_policy ON product_accounting;
DROP POLICY IF EXISTS acc_update_policy ON product_accounting;
DROP POLICY IF EXISTS acc_delete_policy ON product_accounting;
DROP POLICY IF EXISTS supply_read_policy ON supply;
DROP POLICY IF EXISTS supply_write_policy ON supply;

DROP FUNCTION IF EXISTS current_employee_id();

-- 2. Очищаем и заполняем user_mapping
DELETE FROM user_mapping WHERE pg_user = 'user_admin';

INSERT INTO user_mapping (pg_user, employee_id) VALUES
('user_employee',   1),
('user_manager',    2),
('user_accountant', 3)
ON CONFLICT (pg_user) DO UPDATE SET employee_id = EXCLUDED.employee_id;

-- 3. ДАЁМ ПРАВО ЧТЕНИЯ user_mapping ВСЕМ РОЛЯМ
GRANT SELECT ON user_mapping TO 
    role_warehouse_employee, 
    role_warehouse_manager, 
    role_accountant, 
    role_admin;

-- 4. Пересоздаём политики
CREATE POLICY emp_read_policy ON employee FOR SELECT
USING (
    current_user IN ('user_admin', 'user_manager', 'user_accountant') OR
    employee_id = (SELECT employee_id FROM user_mapping WHERE pg_user = current_user)
);

CREATE POLICY acc_read_policy ON product_accounting FOR SELECT
USING (
    current_user IN ('user_admin', 'user_manager', 'user_accountant') OR
    employee_id = (SELECT employee_id FROM user_mapping WHERE pg_user = current_user)
);

CREATE POLICY acc_insert_policy ON product_accounting FOR INSERT
WITH CHECK (
    current_user IN ('user_admin', 'user_manager', 'user_accountant') OR
    employee_id = (SELECT employee_id FROM user_mapping WHERE pg_user = current_user)
);

CREATE POLICY acc_update_policy ON product_accounting FOR UPDATE
USING (
    current_user IN ('user_admin', 'user_manager', 'user_accountant') OR
    employee_id = (SELECT employee_id FROM user_mapping WHERE pg_user = current_user)
)
WITH CHECK (
    employee_id = (SELECT employee_id FROM user_mapping WHERE pg_user = current_user)
);

CREATE POLICY acc_delete_policy ON product_accounting FOR DELETE
USING (current_user IN ('user_admin', 'user_accountant'));

CREATE POLICY supply_read_policy ON supply FOR SELECT
USING (
    current_user IN ('user_admin', 'user_manager', 'user_accountant') OR
    supply_id IN (
        SELECT pa.supply_id
        FROM product_accounting pa
        WHERE pa.employee_id = (SELECT employee_id FROM user_mapping WHERE pg_user = current_user)
    )
);

CREATE POLICY supply_write_policy ON supply FOR ALL
USING (current_user IN ('user_admin', 'user_manager', 'user_accountant'))
WITH CHECK (current_user IN ('user_admin', 'user_manager', 'user_accountant'));

-- 5. Индекс для скорости
CREATE INDEX IF NOT EXISTS idx_user_mapping_pg_user ON user_mapping(pg_user);
