// Skipping function Up(none), it contains poisonous unsupported syntaxes

func @_ProyectoTareas.Migrations.InitialCreate.Down$Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder$(none) -> () loc("C:\\Users\\jonathan.vahos\\OneDrive - SoftwareOne\\Desktop\\Proyecto Practica\\ProyectoTareas\\ProyectoTareas\\Migrations\\20250121153019_InitialCreate.cs" :33 :8) {
^entry (%_migrationBuilder : none):
%0 = cbde.alloca none loc("C:\\Users\\jonathan.vahos\\OneDrive - SoftwareOne\\Desktop\\Proyecto Practica\\ProyectoTareas\\ProyectoTareas\\Migrations\\20250121153019_InitialCreate.cs" :33 :37)
cbde.store %_migrationBuilder, %0 : memref<none> loc("C:\\Users\\jonathan.vahos\\OneDrive - SoftwareOne\\Desktop\\Proyecto Practica\\ProyectoTareas\\ProyectoTareas\\Migrations\\20250121153019_InitialCreate.cs" :33 :37)
br ^0

^0: // SimpleBlock
%1 = cbde.unknown : none loc("C:\\Users\\jonathan.vahos\\OneDrive - SoftwareOne\\Desktop\\Proyecto Practica\\ProyectoTareas\\ProyectoTareas\\Migrations\\20250121153019_InitialCreate.cs" :35 :12) // Not a variable of known type: migrationBuilder
%2 = cbde.unknown : none loc("C:\\Users\\jonathan.vahos\\OneDrive - SoftwareOne\\Desktop\\Proyecto Practica\\ProyectoTareas\\ProyectoTareas\\Migrations\\20250121153019_InitialCreate.cs" :36 :22) // "Tareas" (StringLiteralExpression)
%3 = cbde.unknown : none loc("C:\\Users\\jonathan.vahos\\OneDrive - SoftwareOne\\Desktop\\Proyecto Practica\\ProyectoTareas\\ProyectoTareas\\Migrations\\20250121153019_InitialCreate.cs" :35 :12) // migrationBuilder.DropTable(                  name: "Tareas") (InvocationExpression)
br ^1

^1: // ExitBlock
return

}
