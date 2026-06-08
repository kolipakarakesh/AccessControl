-- Insert Default Roles

IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'Admin')
BEGIN
    INSERT INTO Roles
    (
        RoleName,
        Description,
        IsActive,
        CreatedBy,
        CreatedDate
    )
    VALUES
    (
        'Admin',
        'Full system access',
        1,
        'System',
        GETDATE()
    );
END

IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'Manager')
BEGIN
    INSERT INTO Roles
    (
        RoleName,
        Description,
        IsActive,
        CreatedBy,
        CreatedDate
    )
    VALUES
    (
        'Manager',
        'Manage users and reports',
        1,
        'System',
        GETDATE()
    );
END

IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'User')
BEGIN
    INSERT INTO Roles
    (
        RoleName,
        Description,
        IsActive,
        CreatedBy,
        CreatedDate
    )
    VALUES
    (
        'User',
        'Limited system access',
        1,
        'System',
        GETDATE()
    );
END