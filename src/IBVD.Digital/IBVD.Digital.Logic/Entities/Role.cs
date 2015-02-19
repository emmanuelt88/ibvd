using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace IBVD.Digital.Logic.Entities
{
    public class Role
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public string Codigo { get; private set; }
        public bool Habilitado { get; private set; }
        public ReadOnlyCollection<Operacion> Operaciones { get; internal set; }

        public Role()
        {

        }

        public Role(int id, string nombre, string codigo, bool habilitado)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Codigo = codigo;
            this.Habilitado = habilitado;
            this.Operaciones = new ReadOnlyCollection<Operacion>(new List<Operacion>());
        }


        public Role(int id, string nombre, string codigo, bool habilitado, IList<Operacion> operaciones)
            :this(id, nombre, codigo, habilitado)
        {
            this.Operaciones =new ReadOnlyCollection<Operacion>(operaciones);
        }


        internal void AddOperacion(Operacion operacion)
        {
            var operaciones = Operaciones.ToList();
            operaciones.Add(operacion);
            this.Operaciones = new ReadOnlyCollection<Operacion>(operaciones);
        }
    }
}
