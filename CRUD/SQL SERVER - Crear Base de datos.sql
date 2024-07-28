--- ***********************************************************  ---
--- Creación de Base de datos, con inserts minimos para pruebas  --- 
--- ***********************************************************  ---

CREATE DATABASE CRUD;

USE CRUD;

-- CREAR TABLAS

-- Guarda los tipos de identifacion que manejara el sistema
CREATE TABLE tipos_identificacion (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tipo_identificacion VARCHAR(50) UNIQUE NOT NULL -- Definido por el modelo de negocio
);

-- Contiene los paises actuales a la fecha 28-07-2024
CREATE TABLE paises 
(
    id_codigo_pais CHAR(2) PRIMARY KEY, -- según el estándar de códigos de país de la ISO 3166-1 alfa-2
    nombre_pais VARCHAR(100) NOT NULL,
	codigo_marcacion VARCHAR(10) NULL,-- Ejemplo: '+1', '+57'
);

-- Almacena los usuarios que tendran acceso
CREATE TABLE usuario
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(100) NOT NULL,
	usuario VARCHAR(50) UNIQUE NOT NULL, 
	contrasenia VARCHAR(64) NOT NULL, -- diligenciar en formato SHA-256, Siempre tendra 64 caracteres en formato HEX
	correo_electronico VARCHAR(100) UNIQUE NOT NULL, -- Unico ya que el login se realiza con este valor
	edad TINYINT NOT NULL, -- Tipo de dato TINYINT ya que una persona generalmente no vive mas de 255 años,(Modificable para otros modelos de negocio)
	id_tipo_identificacion INT,
	numero_identificacion VARCHAR(20) NOT NUll,
	FOREIGN KEY (id_tipo_identificacion) REFERENCES tipos_identificacion(id)
);

-- Almacena los clientes que tiene un usuario
CREATE TABLE cliente
(
	id INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
	nombre VARCHAR(100) NOT NULL,
	edad TINYINT NOT NULL,
	id_tipo_identificacion INT,
	numero_identificacion VARCHAR(20) NOT NUll,
	FOREIGN KEY (id_tipo_identificacion) REFERENCES tipos_identificacion(id),
    FOREIGN KEY (id_usuario) REFERENCES usuario(id)
);

-- Almacena las direcciones que tiene un cliente
CREATE TABLE cliente_direccion
(
	id INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT NOT NULL,
    direccion VARCHAR(255) NOT NULL,
    ciudad VARCHAR(100) NOT NULL,
    codigo_postal VARCHAR(10) NULL,
    id_codigo_pais CHAR(2) NOT NULL,
	FOREIGN KEY (id_codigo_pais) REFERENCES paises(id_codigo_pais), 
    FOREIGN KEY (id_cliente) REFERENCES cliente(id) ON DELETE CASCADE,-- DELETE CASCADE, en caso de que el cliente sea eliminado, evita datos huerfanos
);

-- Almacena los telefonos que tiene un cliente
CREATE TABLE cliente_contacto (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_codigo_pais CHAR(2) NOT NULL,
    tipo_telefono VARCHAR(5) NOT NULL CHECK (tipo_telefono IN ('fijo', 'movil')), -- CHECK - Valida que el valor ingresado sea solo fijo o movil
    numero_telefono VARCHAR(15) NOT NULL, -- Limite de 15 ya que un numero de celular actualemnte no tiene mas de 15 digitos (sin contar el numero de marcación)
    FOREIGN KEY (id_codigo_pais) REFERENCES paises(id_codigo_pais),
	FOREIGN KEY (id_cliente) REFERENCES cliente(id) ON DELETE CASCADE,
);

-- Almacena los coreros electronicos que pueda tener un cliente
CREATE TABLE cliente_correo_electronico
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	id_cliente INT,
	direccion_correo VARCHAR(100),
	FOREIGN KEY (id_cliente) REFERENCES cliente(id) ON DELETE CASCADE, -- En caso de que se elimine el cliente se eliminan los correos, evita datos huerfanos
);

-- INSERTAR DATA
-- Lista de Paises con codigo de marcasión segun la ISO 3166-1
INSERT INTO paises (id_codigo_pais, nombre_pais, codigo_marcacion)
VALUES
    ('AF', 'Afganistán', '+93'),
    ('AL', 'Albania', '+355'),
    ('DZ', 'Argelia', '+213'),
    ('AD', 'Andorra', '+376'),
    ('AO', 'Angola', '+244'),
    ('AG', 'Antigua y Barbuda', '+1-268'),
    ('AR', 'Argentina', '+54'),
    ('AM', 'Armenia', '+374'),
    ('AU', 'Australia', '+61'),
    ('AT', 'Austria', '+43'),
    ('AZ', 'Azerbaiyán', '+994'),
    ('BS', 'Bahamas', '+1-242'),
    ('BH', 'Baréin', '+973'),
    ('BD', 'Bangladés', '+880'),
    ('BB', 'Barbados', '+1-246'),
    ('BY', 'Bielorrusia', '+375'),
    ('BE', 'Bélgica', '+32'),
    ('BZ', 'Belice', '+501'),
    ('BJ', 'Benín', '+229'),
    ('BT', 'Bután', '+975'),
    ('BO', 'Bolivia', '+591'),
    ('BA', 'Bosnia y Herzegovina', '+387'),
    ('BW', 'Botswana', '+267'),
    ('BR', 'Brasil', '+55'),
    ('BN', 'Brunéi', '+673'),
    ('BG', 'Bulgaria', '+359'),
    ('BF', 'Burkina Faso', '+226'),
    ('BI', 'Burundi', '+257'),
    ('CV', 'Cabo Verde', '+238'),
    ('KH', 'Camboya', '+855'),
    ('CM', 'Camerún', '+237'),
    ('CA', 'Canadá', '+1'),
    ('CF', 'República Centroafricana', '+236'),
    ('TD', 'Chad', '+235'),
    ('CL', 'Chile', '+56'),
    ('CN', 'China', '+86'),
    ('CO', 'Colombia', '+57'),
    ('KM', 'Comoras', '+269'),
    ('CG', 'Congo', '+242'),
    ('CD', 'República Democrática del Congo', '+243'),
    ('CR', 'Costa Rica', '+506'),
    ('CI', 'Costa de Marfil', '+225'),
    ('HR', 'Croacia', '+385'),
    ('CU', 'Cuba', '+53'),
    ('CY', 'Chipre', '+357'),
    ('CZ', 'República Checa', '+420'),
    ('DK', 'Dinamarca', '+45'),
    ('DJ', 'Yibuti', '+253'),
    ('DM', 'Dominica', '+1-767'),
    ('DO', 'República Dominicana', '+1-809'),
    ('EC', 'Ecuador', '+593'),
    ('EG', 'Egipto', '+20'),
    ('SV', 'El Salvador', '+503'),
    ('GQ', 'Guinea Ecuatorial', '+240'),
    ('ER', 'Eritrea', '+291'),
    ('EE', 'Estonia', '+372'),
    ('SZ', 'Esuatini', '+268'),
    ('ET', 'Etiopía', '+251'),
    ('FJ', 'Fiyi', '+679'),
    ('FI', 'Finlandia', '+358'),
    ('FR', 'Francia', '+33'),
    ('GA', 'Gabón', '+241'),
    ('GM', 'Gambia', '+220'),
    ('GE', 'Georgia', '+995'),
    ('DE', 'Alemania', '+49'),
    ('GH', 'Ghana', '+233'),
    ('GR', 'Grecia', '+30'),
    ('GD', 'Granada', '+1-473'),
    ('GT', 'Guatemala', '+502'),
    ('GN', 'Guinea', '+224'),
    ('GW', 'Guinea-Bisáu', '+245'),
    ('GY', 'Guyana', '+592'),
    ('HT', 'Haití', '+509'),
    ('HN', 'Honduras', '+504'),
    ('HU', 'Hungría', '+36'),
    ('IS', 'Islandia', '+354'),
    ('IN', 'India', '+91'),
    ('ID', 'Indonesia', '+62'),
    ('IR', 'Irán', '+98'),
    ('IQ', 'Irak', '+964'),
    ('IE', 'Irlanda', '+353'),
    ('IL', 'Israel', '+972'),
    ('IT', 'Italia', '+39'),
    ('JM', 'Jamaica', '+1-876'),
    ('JP', 'Japón', '+81'),
    ('JO', 'Jordania', '+962'),
    ('KZ', 'Kazajistán', '+7'),
    ('KE', 'Kenia', '+254'),
    ('KI', 'Kiribati', '+686'),
    ('KP', 'Corea del Norte', '+850'),
    ('KR', 'Corea del Sur', '+82'),
    ('KW', 'Kuwait', '+965'),
    ('KG', 'Kirguistán', '+996'),
    ('LA', 'Laos', '+856'),
    ('LV', 'Letonia', '+371'),
    ('LB', 'Líbano', '+961'),
    ('LS', 'Lesoto', '+266'),
    ('LR', 'Liberia', '+231'),
    ('LY', 'Libia', '+218'),
    ('LI', 'Liechtenstein', '+423'),
    ('LT', 'Lituania', '+370'),
    ('LU', 'Luxemburgo', '+352'),
    ('MO', 'Macao', '+853'),
    ('MK', 'Macedonia del Norte', '+389'),
    ('MG', 'Madagascar', '+261'),
    ('MW', 'Malawi', '+265'),
    ('MY', 'Malasia', '+60'),
    ('MV', 'Maldivas', '+960'),
    ('ML', 'Malí', '+223'),
    ('MT', 'Malta', '+356'),
    ('MH', 'Islas Marshall', '+692'),
    ('MR', 'Mauritania', '+222'),
    ('MU', 'Mauricio', '+230'),
    ('MX', 'México', '+52'),
    ('FM', 'Micronesia', '+691'),
    ('MD', 'Moldavia', '+373'),
    ('MC', 'Mónaco', '+377'),
    ('MN', 'Mongolia', '+976'),
    ('ME', 'Montenegro', '+382'),
    ('MA', 'Marruecos', '+212'),
    ('MZ', 'Mozambique', '+258'),
    ('MM', 'Birmania', '+95'),
    ('NA', 'Namibia', '+264'),
    ('NR', 'Nauru', '+674'),
    ('NP', 'Nepal', '+977'),
    ('NL', 'Países Bajos', '+31'),
    ('NZ', 'Nueva Zelanda', '+64'),
    ('NI', 'Nicaragua', '+505'),
    ('NE', 'Níger', '+227'),
    ('NG', 'Nigeria', '+234'),
    ('NU', 'Niue', '+683'),
    ('NF', 'Isla Norfolk', '+672'),
    ('MP', 'Islas Marianas del Norte', '+1-670'),
    ('NO', 'Noruega', '+47'),
    ('OM', 'Omán', '+968'),
    ('PK', 'Pakistán', '+92'),
    ('PW', 'Palaos', '+680'),
    ('PA', 'Panamá', '+507'),
    ('PG', 'Papúa Nueva Guinea', '+675'),
    ('PY', 'Paraguay', '+595'),
    ('PE', 'Perú', '+51'),
    ('PH', 'Filipinas', '+63'),
    ('PN', 'Islas Pitcairn', '+64'),
    ('PL', 'Polonia', '+48'),
    ('PT', 'Portugal', '+351'),
    ('PR', 'Puerto Rico', '+1-787'),
    ('QA', 'Catar', '+974'),
    ('RE', 'Reunión', '+262'),
    ('RO', 'Rumania', '+40'),
    ('RU', 'Rusia', '+7'),
    ('RW', 'Ruanda', '+250'),
    ('SH', 'Santa Elena', '+290'),
    ('LC', 'Santa Lucía', '+1-758'),
    ('PM', 'San Pedro y Miquelón', '+508'),
    ('VC', 'San Vicente y las Granadinas', '+1-784'),
    ('WS', 'Samoa', '+685'),
    ('SM', 'San Marino', '+378'),
    ('ST', 'Santo Tomé y Príncipe', '+239'),
    ('SA', 'Arabia Saudita', '+966'),
    ('SN', 'Senegal', '+221'),
    ('SC', 'Seychelles', '+248'),
    ('SL', 'Sierra Leona', '+232'),
    ('SG', 'Singapur', '+65'),
    ('SX', 'San Martín', '+1-721'),
    ('SK', 'Eslovaquia', '+421'),
    ('SI', 'Eslovenia', '+386'),
    ('SB', 'Islas Salomón', '+677'),
    ('SO', 'Somalia', '+252'),
    ('ZA', 'Sudáfrica', '+27'),
    ('GS', 'Georgia del Sur', '+500'),
    ('ES', 'España', '+34'),
    ('LK', 'Sri Lanka', '+94'),
    ('SD', 'Sudán', '+249'),
    ('SR', 'Surinam', '+597'),
    ('SE', 'Suecia', '+46'),
    ('CH', 'Suiza', '+41'),
    ('SY', 'Siria', '+963'),
    ('TW', 'Taiwán', '+886'),
    ('TJ', 'Tayikistán', '+992'),
    ('TZ', 'Tanzania', '+255'),
    ('TH', 'Tailandia', '+66'),
    ('TL', 'Timor Oriental', '+670'),
    ('TG', 'Togo', '+228'),
    ('TK', 'Tokelau', '+690'),
    ('TO', 'Tonga', '+676'),
    ('TT', 'Trinidad y Tobago', '+1-868'),
    ('TN', 'Túnez', '+216'),
    ('TR', 'Turquía', '+90'),
    ('TM', 'Turkmenistán', '+993'),
    ('TV', 'Tuvalu', '+688'),
    ('UG', 'Uganda', '+256'),
    ('UA', 'Ucrania', '+380'),
    ('AE', 'Emiratos Árabes Unidos', '+971'),
    ('GB', 'Reino Unido', '+44'),
    ('US', 'Estados Unidos', '+1'),
    ('UY', 'Uruguay', '+598'),
    ('UZ', 'Uzbekistán', '+998'),
    ('VU', 'Vanuatu', '+678'),
    ('VE', 'Venezuela', '+58'),
    ('VN', 'Vietnam', '+84'),
    ('WF', 'Wallis y Futuna', '+681'),
    ('EH', 'Sáhara Occidental', '+212'),
    ('YE', 'Yemen', '+967'),
    ('ZM', 'Zambia', '+260'),
    ('ZW', 'Zimbabue', '+263');

-- Tipos de identificación aceptados por el modelo de negocio
INSERT INTO tipos_identificacion (tipo_identificacion)
VALUES
    ('Cédula de Ciudadanía'), -- id 1
    ('Cédula de Extranjería'),-- id 2
    ('Número de Identificación Tributaria (NIT)'), --id 3
    ('Pasaporte'); --id 4

-- Clientes de prueba
INSERT INTO cliente 
VALUES
('Diego Barreto',77,1,'1111111111'),
('Alexander Moreno',22,2,'CE451745A45');

-- Contactos de prueba
INSERT INTO cliente_contacto 
VALUES
(1,'CO','movil','3120000001'),
(1,'CO','fijo','1555551'),
(2,'CO','movil','3120000342'),
(2,'CO','movil','3120000123');

-- Direcciónes de prueba
INSERT INTO cliente_direccion
VALUES 
(1,'Calle 123 # 15-15', 'Bogota', '100114', 'CO'),
(2,'Calle 000 # 77-77', 'Caracas', '500114', 'VE'),
(2,'Calle 999 # 234-86', 'Morelos', '451114', 'MX');

-- Correos electronicos de prueba
INSERT INTO cliente_correo_electronico
VALUES 
(1,'diego01@domain.com'),
(1,'diego02@domain.com'),
(1,'diego03@domain.com'),
(2,'otro01@domain.com');

-- Usuario para logueo de prueba
INSERT INTO usuario
VALUES 
('Diego B.','Admin','A109E36947AD56DE1DCA1CC49F0EF8AC9AD9A7B1AA0DF41FB3C4CB73C1FF01EA','admin@domain.com','99','1','999999999');