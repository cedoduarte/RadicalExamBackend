
namespace RadicalExam.Models
{
    public class ExcelRecord
    {
        // PRIMER_NOMBRE
        public string FirstName { get; set; }

        // SEGUNDO_NOMBRE
        public string MiddleName { get; set; }

        // APELLIDO_PATERNO
        public string FirstLastName { get; set; }

        // APELLIDO_MATERNO
        public string SecondLastName { get; set; }

        // FECHA_DE_NACIMIENTO
        public string Birthdate { get; set; }

        // RFC
        public string RFC { get; set; }

        // COLONIA_O_POBLACIÓN
        public string Neighborhood { get; set; }

        // DELEGACIÓN_O_MUNICIPIO
        public string DelegationOrMunicipality { get; set; }

        // CIUDAD
        public string City { get; set; }

        // STATE
        public string State { get; set; }

        // C.P.
        public string ZipCode { get; set; }

        // DIRECCION_CALLE_NUMERO"
        public string Address { get; set; }

        // SALDO_ACTUAL
        public decimal CurrentBalance { get; set; }

        // LIMITE_DE_CREDITO
        public decimal CreditLimit { get; set; }

        // SALDO_VENCIDO
        public decimal BalanceDue { get; set; }

        public decimal AvailableBalance { get; set; }
    }
}
