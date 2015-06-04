/*
Navicat SQLite Data Transfer

Source Server         : mapping
Source Server Version : 30714
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30714
File Encoding         : 65001

Date: 2015-06-03 10:01:35
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for DLC_info
-- ----------------------------
DROP TABLE IF EXISTS "main"."DLC_info";
CREATE TABLE "DLC_info" (
"id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"title"  TEXT,
"content"  TEXT,
"category_id"  INTEGER,
"root_id"  INTEGER,
"add_time"  TEXT,
"search_words"  TEXT,
"author"  TEXT
);

-- ----------------------------
-- Records of DLC_info
-- ----------------------------
INSERT INTO "main"."DLC_info" VALUES (15, 1231231, 123121231, 18, 0, '2015-05-28', '', '');
INSERT INTO "main"."DLC_info" VALUES (16, 'hello word', 'hello wordhello wordhello wordhello wordhello wordhello wordhello wordhello wordhello wordhello wordhello wordhello wordhello wordhello wordhello word', 21, 0, '2015-05-29', null, null);
INSERT INTO "main"."DLC_info" VALUES (17, 'The use of revers', 'The use of reverse osmosis is becoming the accepted standard for desalination. Reverse osmosis (RO) system has been used for many years to remove dissolved substances from seawater', 21, 0, '2015-05-29', '', 'skx');
INSERT INTO "main"."DLC_info" VALUES (18, 12312312, 1231, 21, 0, '2015-06-01', '', '');
INSERT INTO "main"."DLC_info" VALUES (19, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (20, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (21, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (22, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (23, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (24, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (25, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (26, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (27, 12312312, 12312312, 21, 0, '2015-06-01', null, null);
INSERT INTO "main"."DLC_info" VALUES (28, 12312312, 12312312, 21, 0, '2015-06-01', '', '');
