namespace SafeScribe_cp05.Enum
{
    public enum UserRole
    {
        /// <summary>
        /// Pode apenas visualizar as suas próprias notas.
        /// </summary>
        Leitor, // Valor padrão é 0

        /// <summary>
        /// Pode criar e editar as suas próprias notas.
        /// </summary>
        Editor, // Valor padrão é 1

        /// <summary>
        /// Possui controle total, podendo visualizar, editar e apagar as notas de qualquer utilizador.
        /// </summary>
        Admin // Valor padrão é 2
    }
}
