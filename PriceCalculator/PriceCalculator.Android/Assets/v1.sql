CREATE TABLE IF NOT EXISTS `product` (
	`id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	`name`	TEXT,
	`category`	TEXT,
	`itemUsed`	TEXT,
	`costPrice`	NUMERIC,
	`profitPercent`	NUMERIC,
	`sellingPrice`	NUMERIC,
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
	`rate`	INTEGER,
	`unit`	TEXT
);
CREATE TABLE IF NOT EXISTS `category` (
	`id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	`name`	TEXT
);
CREATE TABLE IF NOT EXISTS 'info' (
	'key' TEXT,
	'value' TEXT
);