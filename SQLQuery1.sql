USE master;
DROP DATABASE eBookStore;

CREATE DATABASE eBookStore4;
USE eBookStore4;


CREATE TABLE categories(
    id INT IDENTITY(1000, 1) PRIMARY KEY,
    name NVARCHAR(255),
    created_at DATETIME,
    is_deleted BIT DEFAULT 0
);

CREATE TABLE users
(
    id VARCHAR(45) PRIMARY KEY,
    phone VARCHAR(45) UNIQUE,
    mail VARCHAR(45) UNIQUE,
    password VARCHAR(255),
    address NVARCHAR(255),
    avatar VARCHAR(255),
    display_name NVARCHAR(255),
    created_at DATETIME,
    is_deleted BIT DEFAULT 0,
    is_active BIT DEFAULT 1,
);

CREATE TABLE roles
(
	id VARCHAR(45) PRIMARY KEY,
	name VARCHAR(45)
);

CREATE TABLE user_roles
(
	id VARCHAR(45) PRIMARY KEY,
	user_id VARCHAR(45),
	role_id VARCHAR(45),
	role_name VARCHAR(45),
	FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
	FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE CASCADE,
);

CREATE TABLE products(
    id INT IDENTITY(1000, 1) PRIMARY KEY,
    category_id INT,
    name NVARCHAR(255),
    quantity INT DEFAULT 0,
    page INT,
    is_deleted BIT DEFAULT 0,
    on_stock BIT DEFAULT 1,
    created_at DATETIME,
    price MONEY,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
);

CREATE TABLE product_images(
    id INT IDENTITY(1000, 1) PRIMARY KEY,
    product_id INT,
    link VARCHAR(255),
    FOREIGN KEY (product_id) REFERENCES products(id)  ON DELETE CASCADE
);

CREATE TABLE orders(
    id INT IDENTITY(1000, 1) PRIMARY KEY,
    user_id VARCHAR(45),
    product_id INT,
    quantity INT,
    price MONEY,
    delivery_address VARCHAR(255),
    method VARCHAR(45),
    status VARCHAR(45),
    order_at DATETIME,
    update_at DATETIME,
    total MONEY,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products ON DELETE CASCADE
);

CREATE TABLE reviews(
    id INT IDENTITY(1000, 1) PRIMARY KEY,
    product_id INT,
    user_id VARCHAR(45),
    comment NVARCHAR(255),
    star INT,
    create_at DATETIME,
    is_deleted BIT DEFAULT 0,
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
);

CREATE TABLE review_images(
    id INT IDENTITY(1000, 1) PRIMARY KEY,
    review_id INT,
    create_at DATETIME,
    is_deleted BIT DEFAULT 0,
    FOREIGN KEY (review_id) REFERENCES reviews(id) ON DELETE CASCADE
);


