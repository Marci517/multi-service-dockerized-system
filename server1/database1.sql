DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'restaurant_db') THEN
        CREATE DATABASE restaurant_db;
    END IF;
END
$$;


\c restaurant_db;

CREATE TABLE Restaurant (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    address VARCHAR(255) NOT NULL,
    rating DECIMAL(2, 1) NOT NULL CHECK (rating >= 0 AND rating <= 5)
);

INSERT INTO Restaurant (name, address, rating) VALUES
('La Bella Napoli', '123 Main Street, Naples', 4.5),
('Tokyo Sushi', '456 Elm Avenue, Tokyo', 4.8),
('Burger Heaven', '789 Oak Boulevard, New York', 4.2);
