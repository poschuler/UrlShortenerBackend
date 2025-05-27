CREATE EXTENSION IF NOT EXISTS btree_gist;

CREATE TABLE token_ranges (
    id BIGSERIAL PRIMARY KEY,
    range_start BIGINT NOT NULL,
    range_end BIGINT NOT NULL,
    server_id VARCHAR(255),
    request_server_id VARCHAR(255),
    created_on_utc timestamptz DEFAULT CURRENT_TIMESTAMP NOT NULL,

    CONSTRAINT range_positive_start CHECK (range_start > 0),
    CONSTRAINT range_end_after_start CHECK (range_end >= range_start),
    CONSTRAINT no_overlapping_ranges EXCLUDE USING gist (int8range(range_start, range_end, '[]') WITH &&)
);

CREATE INDEX IF NOT EXISTS idx_token_ranges_server_id ON token_ranges(server_id);

CREATE INDEX IF NOT EXISTS idx_token_ranges_range_end ON token_ranges(range_end);