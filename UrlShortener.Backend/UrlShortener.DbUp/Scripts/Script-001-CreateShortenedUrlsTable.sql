CREATE TABLE shortened_urls (
    short_code VARCHAR(255) PRIMARY KEY,
    long_url TEXT NOT NULL,
    created_on_utc timestamptz DEFAULT CURRENT_TIMESTAMP NOT NULL
);
