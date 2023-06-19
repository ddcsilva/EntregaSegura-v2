// Angular imports
import { Router } from "@angular/router";
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTable, MatTableDataSource } from "@angular/material/table";

// Model imports
import { Morador } from "src/app/models/morador";

// Service imports
import { MoradorService } from "src/app/services/morador/morador.service";

// Component imports
import { ExclusaoDialogComponent } from "src/app/shared/components/exclusao-dialog/exclusao-dialog.component";

// Library imports
import { ToastrService } from "ngx-toastr";
import { Subject, takeUntil } from "rxjs";
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-morador-lista',
  templateUrl: './morador-lista.component.html',
  styleUrls: ['./morador-lista.component.scss']
})
export class MoradorListaComponent implements OnInit, OnDestroy {
  public titulo: string = 'Lista de Moradores';
  public colunasExibidas: string[] = ['id', 'nome', 'ramal', 'nomeCondominio', 'descricaoUnidade', 'actions'];
  private listaMoradores: Morador[] = [];
  public dataSource = new MatTableDataSource<Morador>(this.listaMoradores);
  private destroy$ = new Subject<void>();

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatTable, { static: true }) table!: MatTable<Morador>;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private router: Router,
    private moradorService: MoradorService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.spinner.show();
    this.obterLista();
    this.dataSource.paginator = this.paginator;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public excluirMorador(id: number) {
    const dialogRef = this.dialog.open(ExclusaoDialogComponent);
    dialogRef.afterClosed().subscribe(confirmacaoExclusao => {
      if (confirmacaoExclusao) {
        this.moradorService.excluir(id.toString()).subscribe({
          next: () => {
            this.toastr.success('Morador excluído com sucesso', 'Sucesso!');
            this.obterLista();
          },
          error: (error: any) => {
            this.exibirErros(error);
          }
        });
      }
    });
  }

  public editarMorador(id: number): void {
    this.router.navigate(['moradores/detalhe', id]);
  }

  public aplicarFiltro(evento: Event) {
    const filtro = (evento.target as HTMLInputElement).value;
    this.dataSource.filter = filtro.trim().toLowerCase();
  }

  private obterLista() {
    this.moradorService.obterTodos().pipe(takeUntil(this.destroy$)).subscribe({
      next: (moradores: Morador[]) => {
        this.listaMoradores = moradores;
        this.dataSource = new MatTableDataSource<Morador>(this.listaMoradores);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.table.renderRows();
        this.spinner.hide();
      },
      error: (erro: any) => {
        this.exibirErros(erro);
        this.spinner.hide();
      }
    });
  }

  private exibirErros(erro: any) {
    if (typeof erro === 'string') {
      this.toastr.error(erro, 'Erro!');
    } else if (erro instanceof Array) {
      erro.forEach(mensagemErro => this.toastr.error(mensagemErro, 'Erro!'));
    } else {
      this.toastr.error(erro.message || 'Erro ao excluir', 'Erro!');
    }
  }
}