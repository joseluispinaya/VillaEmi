<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.Master" AutoEventWireup="true" CodeBehind="ListaEstudiantes.aspx.cs" Inherits="CapaPresentacion.ListaEstudiantes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
    <div class="col-lg-4">
        <div class="card">
            <div class="card-body">
                <h4 class="mt-0 mb-20"><i class="fa fa-filter text-primary mr-3"></i>Filtros de Búsqueda</h4>

                <div class="form-group mb-25">
                    <label for="cboUnidadEducativa" class="font-600 text-muted mb-10">Unidad Educativa / Colegio</label>
                    <div class="input-group">
                        <select class="form-control" id="cboUnidadEducativa">
                        </select>
                    </div>
                    <small class="text-muted d-block">Selecciona una institución para listar a sus estudiantes.</small>
                </div>

                <hr class="mt-20 mb-20" style="border-top: 1px solid #eeeeee;" />

                <h4 class="mt-0 mb-2"><i class="fa fa-file text-success mr-2"></i>Acciones Disponibles</h4>

                <p class="text-muted font-13 m-b-20">
                    Puedes exportar el reporte consolidado de los tests vocacionales y las recomendaciones de la IA pertenecientes a la unidad educativa seleccionada.
                </p>

                <button id="btnGenerarReporte" type="button" class="btn btn-success btn-md waves-effect waves-light w-100" disabled>
                    <i class="fa fa-file-pdf mr-2"></i>Generar Reporte Completo
                </button>
            </div>
        </div>
    </div>

    <div class="col-lg-8">
        <div class="card">
            <div class="card-body">
                <div class="mb-3 mt-0">
                    <span class="badge badge-primary float-right p-2" id="lblContadorEstudiantes" style="font-size: 12px; border-radius: 4px;">0 Alumnos</span>
                    <h4 class="m-0"><i class="fa fa-users text-primary mr-2"></i>Estudiantes por Unidad Educativa</h4>
                </div>

                <table id="tbDatas" class="table table-hover m-0">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Estudiantes</th>
                            <th>Detalle</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="rowPlaceholder">
                            <td colspan="3" class="text-center text-muted pt-30 pb-30">
                                <i class="fas fa-school fa-2x mb-3 d-block text-success"></i>
                                Por favor, seleccione una Unidad Educativa en el panel izquierdo para ver los registros.
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="js/ListaEstudiantes.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>

</asp:Content>
