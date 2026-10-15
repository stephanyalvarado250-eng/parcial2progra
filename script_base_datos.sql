-- 1. Crear la base de datos
IF DB_ID('ParcialProgramacion1') IS NULL
BEGIN
    CREATE DATABASE ParcialProgramacion1;
END;
GO

USE ParcialProgramacion1;
GO

-- 2. Crear la tabla Productos
CREATE TABLE dbo.Productos
(
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Categoria NVARCHAR(50) NOT NULL,
    Precio DECIMAL (10,2) NOT NULL,
    Existencias INT NOT NULL
);
GO

-- 3. Registrar al menos tres productos de ejemplo
INSERT INTO dbo.Productos (Nombre, Categoria, Precio, Existencias)
VALUES 
('Laptop HP 15', 'Electrónica', 650.00, 10),
('Teclado Mecánico', 'Accesorios', 45.50, 25),
('Mouse Inalámbrico', 'Accesorios', 20.00, 50);
GO
