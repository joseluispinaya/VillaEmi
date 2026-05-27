<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.Master" AutoEventWireup="true" CodeBehind="Configuraciones.aspx.cs" Inherits="CapaPresentacion.Configuraciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
        <div class="col-lg-4">
            <div class="card text-center">
                <div class="card-body pt-30 pb-30">
                    <div class="text-primary m-b-20">
                        <i class="fa fa-graduation-cap fa-4x"></i>
                    </div>
                    <h4 class="mb-2">Base de Conocimiento IA</h4>
                    <p class="text-muted mb-3 font-12" style="line-height: 1.3;">
                        Gestiona el catálogo de <b>carreras universitarias en Bolivia</b>. 
                El modelo de inteligencia artificial analizará los resultados del test vocacional de cada estudiante y cruzará sus aptitudes con esta base de datos para generar sus recomendaciones profesionales.
                    </p>

                    <button type="button" id="btnNuevoReg" class="btn btn-primary btn-md waves-effect waves-light w-100">
                        <i class="fa fa-plus mr-2"></i>Registrar Nueva Carrera
                    </button>
                </div>
            </div>

            <div class="card bg-info text-white">
                <div class="card-body">
                    <div class="media">
                        <div class="media-body">
                            <h5 class="text-white mt-0 mb-2">Motor de Recomendación</h5>
                            <p class="mb-0 font-13">Catálogo enlazado y activo.</p>
                        </div>
                        <div class="ml-auto">
                            <i class="fa fa-check-circle fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-8">
            <div class="card">
                <div class="card-body">
                    <h5 class="mt-0 mb-3">Directorio de Carreras Habilitadas</h5>

                    <table id="tbDatas" class="table table-hover table-sm m-0">
                        <thead class="bg-light">
                            <tr>
                                <th>Id</th>
                                <th>Carreras</th>
                                <th>Estado</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="mdData" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="myModalLabel" class="modal-title"><i class="fa fa-book mr-2"></i>Gestión de Carrera</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="txtCarrera">Nombre de la Carrera</label>
                        <input type="text" class="form-control form-control-sm model" id="txtCarrera" name="Nombres" placeholder="Nombre de la Carrera">
                    </div>
                    <div class="form-group">
                        <label for="cboEstado">Estado</label>
                        <select class="form-control form-control-sm" id="cboEstado">
                            <option value="1">Activo (Visible para la IA)</option>
                            <option value="0">Inactivo (Oculto)</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer justify-content-between">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                    <button type="button" class="btn btn-primary" id="btnGuardarCambios">Guardar</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="js/Configuraciones.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>

</asp:Content>
