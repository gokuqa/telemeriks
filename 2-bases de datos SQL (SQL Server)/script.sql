
-- Dispositivos:
  -- DispositivoID: Identificador único del dispositivo (clave primaria).
  -- Nombre: Nombre del dispositivo.
  -- Tipo: Tipo de dispositivo (por ejemplo, sensor de temperatura, humedad, etc.).
-- FechaCreacion: Fecha de creación del dispositivo.
-- Eventos:
  -- EventoID: Identificador único del evento (clave primaria).
  -- DispositivoID: Referencia al dispositivo asociado (clave foránea).
  -- FechaEvento: Fecha en la que ocurrió el evento.
  -- Valor: Valor del evento, que puede ser un número o texto (por ejemplo, temperatura, humedad, etc.).

  -- Consulta para obtener la cantidad de eventos por dispositivo

-- Creación de la tabla Dispositivos
CREATE TABLE Dispositivos
(
    DispositivoID INT PRIMARY KEY IDENTITY(1,1),  -- ID único para cada dispositivo
    Nombre VARCHAR(100) NOT NULL,                  -- Nombre del dispositivo
    Tipo VARCHAR(50) NOT NULL,                     -- Tipo del dispositivo
    FechaCreacion DATETIME DEFAULT GETDATE()       -- Fecha de creación
);

-- Creación de la tabla Eventos
CREATE TABLE Eventos
(
    EventoID INT PRIMARY KEY IDENTITY(1,1),       -- ID único para cada evento
    DispositivoID INT,                             -- ID del dispositivo (clave foránea)
    FechaEvento DATETIME DEFAULT GETDATE(),        -- Fecha en que ocurrió el evento
    Valor DECIMAL(10, 2) NOT NULL,                 -- Valor del evento (por ejemplo, temperatura, humedad)
    FOREIGN KEY (DispositivoID) REFERENCES Dispositivos(DispositivoID)  -- Relación con la tabla Dispositivos
);

-- Inserción de ejemplo en la tabla Dispositivos
INSERT INTO Dispositivos (Nombre, Tipo)
VALUES ('SensorTemperatura01', 'Temperatura'),
       ('SensorHumedad01', 'Humedad');

-- Inserción de ejemplo en la tabla Eventos
INSERT INTO Eventos (DispositivoID, Valor)
VALUES (1, 23.5),  -- Evento para el dispositivo con ID 1 (SensorTemperatura01)
       (2, 65.3);  -- Evento para el dispositivo con ID 2 (SensorHumedad01)




SELECT d.DispositivoID, d.Nombre, d.Tipo, COUNT(e.EventoID) AS CantidadEventos
FROM Dispositivos d
LEFT JOIN Eventos e ON d.DispositivoID = e.DispositivoID
GROUP BY d.DispositivoID, d.Nombre, d.Tipo;
