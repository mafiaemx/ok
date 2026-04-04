

create table sclad(
id serial primary key,
name text not null,
address text
);

create table products(
product_id serial primary key,
name text not null,
amount numeric nott null(amount >=0),
unit text not null
);

create table delivery_points(
point_id serial primary key,
name text not null,
adress text
);

create table zalusky(
zalusky_id serial primary key,
id int references,
sclad(id) on delete cascade,
pr

)

ALTER TABLE products ADD COLUMN expiration_date DATE;



UPDATE zalusky SET quantity = 50 WHERE sclad_id = 1;
UPDATE zalusky SET quantity = 30 WHERE sclad_id = 3;
