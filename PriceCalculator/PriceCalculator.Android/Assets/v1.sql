CREATE TABLE IF NOT EXISTS `product` (
	`id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	`name`	TEXT,
	`category`	TEXT,
	`costPrice`	REAL,
	`profitPercent`	NUMERIC,
	`sellingPrice`	REAL,
	`imgName`	TEXT
);
CREATE TABLE IF NOT EXISTS `party` (
	`id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	`name`	TEXT,
	`profitPercent`	INTEGER
);
CREATE TABLE IF NOT EXISTS `item` (
	`id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	`categoryId`	TEXT,
	`name`	TEXT,
	`rate`	REAL,
	`unit`	TEXT
);
CREATE TABLE IF NOT EXISTS `itemsUsed` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT,
	`productId` TEXT,
	`itemId` TEXT,
	`type` TEXT,
	`quantity` INTEGER,
	`unit` TEXT,
	`price` REAL,
	`total` REAL
); 
CREATE TABLE IF NOT EXISTS `category` (
	`id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	`name`	TEXT
);
CREATE TABLE IF NOT EXISTS 'info' (
	'key' TEXT,
	'value' TEXT
);