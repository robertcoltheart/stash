
# 1. Database file

## 1.1 Pages
Pages are 8192 bytes each. 

Pages consist of:

- Header page
- Free list page
- Collection page
- Index page

Each page contains a 32-byte header using the following format:

Offset | Size | Description
-- | -- | --
0 | 4 | Page ID
4 | 1 | Page type (Empty = 0, Header = 1, Collection = 2, Index = 3, Data = 4)
5 | 4 | Previous page ID (max for none)
9 | 4 | Next page ID (max for none)
13 | 1 | The index in the free list slot, for data and index pages, 0-4 (255 if not in use)
14 | 4 | Transaction ID
18 | 1 | In WAL, used as the last transaction page and confirmed on disk (0 or 1)
19 | 4 | For data pages, sets the page ID of the collection
23 | 1 | Number of items on this page
24 | 2 | Used bytes on this page excluding header/footer
26 | 2 | Fragmented bytes on this page (free blocks separating used blocks)
28 | 2 | Next free position, after which there is no fragmentation
30 | 2 | Highest used index slot, or 255 for empty

## 1.2 Database Header

Offset | Size | Description
-- | -- | --
32 | 27 | Value is ** This is a LiteDB file **
59 | 1 | File version (8)
60 | 4 | Page ID of free empty page list (max for none)
64 | 4 | Last created page ID, used when there is no free page inside file
68 | 4 | Creation time (in ticks)
76 | 4 | User version (used to detect database changes)
80 | 4 | Collation LCID
84 | 4 | Collation sort options for strings
88 | 4 | Timeout for waiting for unlock (in seconds)
96 | 1 | Use UTC dates (1 or 0)
97 | 4 | Auto checkpoint limit in pages. Zero disables automatic checkpointing.
101 | 8 | Max limit of database file
192 | var | BSON document of collection names and ID pairs (eg. `{'users': 1}`)

## 1.3 Collection page

Offset | Size | Description
-- | -- | --
32 | 4 | Free page index 1
36 | 4 | Free page index 2
40 | 4 | Free page index 3
44 | 4 | Free page index 4
48 | 4 | Free page index 5
52 | 44 | Reserved
96 | 1 | Number of indexes (max 255 per collection)
97 | var * number of indexes | Collection index blocks

## 1.4 Index page

Offset | Size | Description
-- | -- | --
32 | var | Index blocks and block addresses

## 1.5 Data page

Offset | Size | Description
-- | -- | --
32 | var | Data blocks and block addresses

## 1.6 Page address
Page address consists of a 4-byte page ID plus a 1-byte index within the page

Offset | Size | Description
-- | -- | --
0 | 4 | Page ID
4 | 1 | Index of block within page

## 1.7 Page block lookup
Each end of the page has an array of 4-byte addresses of blocks, each containing the length and address within the block. Number of the items can be obtained from the page header, offset 23.

Offset | Size | Description
-- | -- | --
0 | 2 | Length of block
2 | 2 | Position in page

## 1.8 Blocks
Blocks are contained within the page and can be of the following types:

### 1.8.1 Data block
Offset | Size | Description
-- | -- | --
0 | 1 | Block is extended when it's the 2nd or higher page for the data (0 or 1)
1 | 5 | Page address of next block
5 | length | BSON data within the block

### 1.8.2 Index block
Offset | Size | Description
-- | -- | --
0 | 1 | Index slot reference in collection
1 | 1 | Skip-list level (0-31)
2 | 5 | Page address of the data block
7 | 5 | Page address of next node
12 | level * 5 * 2 | List of previous and next nodes (using page address) in skip list (10 bytes per pair)
level * 10 | var | Value of the index key using BSON

### 1.8.3 Collection block
Offset | Size | Description
-- | -- | --
0 | 1 | Slot index of the node
1 | 1 | Index type (Skip list = 0)

Following the block header, the following values are written:

Size | Description
-- | --
var | C-string of the index name
var | C-string of the index expression ($-prefixed value)
1 | Whether the index is unique
5 | Page address of the head page for the index
5 | Page address of the tail page
1 | Max level of the collection
4 | Free index page linked-list