USE PlugAndPlay

BEGIN TRANSACTION;

    DECLARE @type_string INT = 0;
    DECLARE @type_int INT = 0;
    DECLARE @type_currency INT = 0;
    DECLARE @type_complex INT = 0;

    SELECT @type_string = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'string';
    SELECT @type_int = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'int';
    SELECT @type_currency = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'currency';
    SELECT @type_complex = id from dbo.BMA_FIELD_SCHEMA_TYPES where name = 'complex';

    -- INSERT REQUEST SCHEMA
    DECLARE @request_type VARCHAR(2) = 'FS';
    DECLARE @request_origin VARCHAR(3) = 'SAP';

    INSERT INTO dbo.BMA_REQUEST_SCHEMA(Type, Origin, Name, Display, Active, AllowComments, ApprovalRuleId, CreateDate, UpdateDate) 
    values (@request_type, @request_origin, 'serviceSheet', 'Folha de Serviço', 0, 0, 1, GETDATE(), GETDATE());

    DECLARE @request_schema_id INT = 0;
    SELECT @request_schema_id = id from dbo.BMA_REQUEST_SCHEMA where Type = @request_type and Origin = @request_origin;

    -- INSERT TAB_SCHEMA
    INSERT INTO dbo.BMA_TAB_SCHEMA(RequestSchemaId, Name, Display, [Order], CreateDate, UpdateDate) 
    values (@request_schema_id, 'request', 'request', 0, GETDATE(), GETDATE()),
           (@request_schema_id, 'requestItems', 'requestItems', 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline', 'timeline', 2, GETDATE(), GETDATE());

    DECLARE @tab_schema_request_id INT = 0;
    SELECT @tab_schema_request_id = id from dbo.BMA_TAB_SCHEMA where Name = 'request'
    
    DECLARE @tab_schema_itens_id INT = 0;
    SELECT @tab_schema_itens_id = id from dbo.BMA_TAB_SCHEMA where Name = 'requestItems'

    DECLARE @tab_schema_timeline_id INT = 0;
    SELECT @tab_schema_timeline_id = id from dbo.BMA_TAB_SCHEMA where Name = 'timeline'

    -- INSERT FIELDS_SCHEMA
    INSERT INTO dbo.BMA_FIELD_SCHEMA(RequestSchemaId, Name, TabSchemaId, FieldSchemaTypesId, Required, Display, Visible, [Order], CreateDate, UpdateDate)
    values (@request_schema_id, 'request.field_001', @tab_schema_request_id,  @type_string, 1, 'Order', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'request.field_002', @tab_schema_request_id,  @type_currency, 1, 'Value', 1, 2, GETDATE(), GETDATE()),
           (@request_schema_id, 'request.field_003', @tab_schema_request_id,  @type_string, 1, 'Company', 1, 3, GETDATE(), GETDATE()),
           (@request_schema_id, 'request.field_004', @tab_schema_request_id,  @type_string, 1, 'City', 1, 4, GETDATE(), GETDATE()),
           (@request_schema_id, 'requestItems.field_000', @tab_schema_itens_id,  @type_complex, 0, 'Itens', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'requestItems.field_001', @tab_schema_itens_id,  @type_string, 1, 'Code', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'requestItems.field_002', @tab_schema_itens_id,  @type_string, 1, 'Description', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'requestItems.field_003', @tab_schema_itens_id,  @type_string, 1, 'Quantity', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'requestItems.field_004', @tab_schema_itens_id,  @type_string, 1, 'Unit', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline.field_000', @tab_schema_timeline_id,  @type_complex, 0, 'Timeline', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline.field_001', @tab_schema_timeline_id,  @type_string, 1, 'Description', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline.field_002', @tab_schema_timeline_id,  @type_string, 1, 'User', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline.field_003', @tab_schema_timeline_id,  @type_string, 1, 'Create Date', 1, 1, GETDATE(), GETDATE()),
           (@request_schema_id, 'timeline.field_004', @tab_schema_timeline_id,  @type_string, 1, 'Quantity', 1, 1, GETDATE(), GETDATE());
           
    DECLARE @complex_items_field_schema INT = 0;
    SELECT @complex_items_field_schema = id from dbo.BMA_FIELD_SCHEMA where Name = 'requestItems.field_000'

    update dbo.BMA_FIELD_SCHEMA set Parent = @complex_items_field_schema where name in ('requestItems.field_001', 'requestItems.field_002', 'requestItems.field_003', 'requestItems.field_004')

    DECLARE @complex_timeline_field_schema INT = 0;
    SELECT @complex_timeline_field_schema = id from dbo.BMA_FIELD_SCHEMA where Name = 'timeline.field_000'

    update dbo.BMA_FIELD_SCHEMA set Parent = @complex_timeline_field_schema where name in ('timeline.field_001', 'timeline.field_002', 'timeline.field_003', 'timeline.field_004')

COMMIT TRANSACTION;