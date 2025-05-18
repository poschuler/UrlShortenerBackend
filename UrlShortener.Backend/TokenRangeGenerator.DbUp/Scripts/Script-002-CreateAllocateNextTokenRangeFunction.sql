CREATE OR REPLACE FUNCTION allocate_next_token_range(
    p_server_id VARCHAR(255),
    p_range_size BIGINT DEFAULT 1000
)
RETURNS TABLE (allocated_range_start BIGINT, allocated_range_end BIGINT, message TEXT) AS $$
DECLARE
    max_existing_end BIGINT;
    new_start BIGINT;
    new_end BIGINT;
BEGIN
    LOCK TABLE token_ranges IN EXCLUSIVE MODE;

    SELECT MAX(tr.range_end) INTO max_existing_end FROM token_ranges tr;

    IF max_existing_end IS NULL THEN
        new_start := 1;
    ELSE
        new_start := max_existing_end + 1;
    END IF;

    IF p_range_size < 1 OR p_range_size > 10000 THEN
        RETURN QUERY SELECT NULL::BIGINT, NULL::BIGINT, 'Error: Tamaño de rango inválido (1-100000).'::TEXT;
        RETURN;
    END IF;
    new_end := new_start + p_range_size - 1;

    INSERT INTO token_ranges (range_start, range_end, server_id)
    VALUES (new_start, new_end, p_server_id)
    RETURNING token_ranges.range_start, token_ranges.range_end
    INTO allocated_range_start, allocated_range_end;

    RETURN QUERY SELECT allocated_range_start, allocated_range_end, 'Rango asignado exitosamente.'::TEXT;

END;
$$ LANGUAGE plpgsql;