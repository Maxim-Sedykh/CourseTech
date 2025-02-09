import { Container, Row, Col, Button, ListGroup, Card } from "react-bootstrap";
import { Link } from "react-router-dom";
import { AuthService } from "../services/auth-service";
import { ApiPaths } from "../constants/api-paths";
import { useModal } from "../context/ModalContext";
import { CoursePlanModalContent } from "../modal/ModalContent/CoursePlanModalContent";
import { DocumentationModalContent } from "../modal/ModalContent/DocumentationModalContent";

export function HomePage() {

    const { open } = useModal();
    const modalHandlers = {
            documentation: () => open(<DocumentationModalContent />, 'Документация'),
            coursePlan: () => open(<CoursePlanModalContent />, 'План курса')
        };

    const authService = new AuthService(ApiPaths.AUTH_API_PATH);

    return (
      <Container fluid>
        <Container className="px-1 pb-2 mb-0">
          <Row className="p-4 main-bodyheader d-flex justify-content-between mt-4 br-40">
            <Col md={9} sm={12} className="px-3">
              <p className="text-white mt-2 fs-2">Курс по СУБД MS SQL Server c изучением основ языка запросов T-SQL</p>
              <p className="text-white fs-5 my-4 text-justify">
                Изучите работу с базами данных с СУБД MS SQL Server и языком запросов T-SQL: теория и практика с разборами правильных ответов.
                Наша система практических заданий позволит вам легко и доступно понять учебный материал
                Учите T-SQL быстро и эффективно!
              </p>
              <div className="text-center">
                {authService.isAuthenticated() ? (
                  <Link to="/profile" className="btn d-block bd-cyan-500 br-40 w-50 mt-4 mb-3">
                    Перейти к обучению
                  </Link>
                ) : (
                  <Link to="/register" className="btn d-block bd-cyan-500 br-40 w-50 mt-4 mb-3">
                    Перейти к обучению
                  </Link>
                )}
              </div>
            </Col>
            <Col md={3} sm={12}>
              <p className="text-center text-white fs-4 mt-3">
                Курс содержит:
              </p>
              <div className="border border-white br-40 w-100 text-white mx-auto mt-5 px-3 py-2 lh-lg w-100 fs-6" style={{ height: '115px' }}>
                Занятия: 10<br />
                Всего заданий: 91<br />
                Практические задания: 4
              </div>
              <div className="text-center">
                <Button className="bd-cyan-500 br-40 mt-4" onClick={modalHandlers.coursePlan}>
                  План обучения 
                </Button>
              </div>
            </Col>
          </Row>
          <Row className="ms-4 mt-0">
            <Col md={12} sm={12} className="text-center">
              <ListGroup horizontal className="text-center list-itd">
              <ListGroup.Item style={{ borderRadius: '0px 0px 0px 20px', background: '#030305' }} className="border border-info"><a className="link text-white text-decoration-none" href="#section-2">После обучения вы сможете</a></ListGroup.Item>
              <ListGroup.Item style={{ borderRadius: '0px 0px 20px 0px', background: '#030305' }} className="border border-info"><a className="link text-white text-decoration-none" href="#section-3">О курсе...</a></ListGroup.Item>
            </ListGroup>
          </Col>
        </Row>
      </Container>
      <Container fluid>
        <Container className="ps-0">
          <Row className="mt-5">
            <p className="text-black fs-2">
              Чему вы научитесь на этом курсе
            </p>
            <Col md={4} sm={12}>
              <div>
                <div className="about-course-part-imgdiv mx-auto mb-3">
                  <img className="img-fluid rounded-circle w-100 h-100 main-page-pic2" />
                </div>
                <p>Cоздавать так называемые «программы» внутри базы данных и управлять алгоритмами бизнес-логики</p>
              </div>
            </Col>
            <Col md={4} sm={12}>
              <div>
                <div className="about-course-part-imgdiv mx-auto mb-3">
                  <img className="img-fluid rounded-circle w-100 h-100 main-page-pic3"/>
                </div>
                <p>Основам работы с языком SQL в Среде СУБД Microsoft SQL Server</p>
              </div>
            </Col>
            <Col md={4} sm={12}>
              <div>
                <div className="about-course-part-imgdiv mx-auto mb-3">
                  <img className="img-fluid rounded-circle w-100 h-100 main-page-pic4" />
                </div>
                <p>Формулировать, создавать и отлаживать различные типы запросов T-SQL</p>
              </div>
            </Col>
          </Row>
          <Row className="mt-5">
            <p id="section-3" className="text-black fs-2">
              О курсе
            </p>
            <Col md={7} sm={12}>
              <Card className="px-5 mt-3 br-40 lh-lg" style={{ minHeight: '350px' }}>
                <Card.Body>
                <p className="fs-5 m-auto">
                    В этом курсе вы научитесь работать: с базой данных, путём создания и чтения различных запросов к базам данных
                    в системе управления базами данных (СУБД) "Microsoft SQL Server 2019" и с основами языка запросов T-SQL.
                    <div className="text-center">
                      {authService.isAuthenticated() ? (
                        <Link to="/user/profile" className="btn br-40 bd-cyan-500 fs-5">
                          Продолжите обучение!
                        </Link>
                      ) : (
                        <Link to="/register" className="btn br-40 bd-cyan-500 fs-5">
                          Зарегистрируйтесь и начните обучение!
                        </Link>
                      )}
                    </div>
                  </p>
                </Card.Body>
              </Card>
            </Col>
            <Col md={5} sm={12} className="p-3" style={{ height: '350px' }}>
              <div className="w-100" style={{ height: '350px' }}>
                <img className="img-fluid h-100 br-40 main-page-pic5" style={{ height: '350px' }} src="/images/MainPagePic5.jpg"/>
              </div>
            </Col>
          </Row>
          <Row className="mt-5">
            <p id="section-1" className="text-black fs-2">
              Для кого этот курс
            </p>
            <p className="fs-5 mt-4 lh-base">
              Этот курс для новичков в SQL и для людей, которые имеют некоторые
              знания в сфере работы с данными, в частности языка SQL
              Если вы новичок в изучении БД, то для прохождения этого курса рекомендуется ознакомиться со справочным материалом.
            </p>
            <Button onClick={modalHandlers.documentation} className="bd-cyan-500 text-white my-3 ms-4 br-40" style={{ border: 'none', maxWidth: '200px' }}>
              Документация   {/* Replace placeholder button logic */}
            </Button>
          </Row>
          <Row className="mt-5">
            <p id="section-2" className="text-black fs-2">
              После обучения вы сможете
            </p>
            <ul className="mt-3 ms-5">
              <li>Работать в среде СУБД "MS SQL Server"</li>
              <li>Писать сложные расширенные запросы используя различные функции языка T-SQL</li>
              <li>Создавать запросы для выполнения операций над таблицами</li>
              <li>Выполнять транзакции</li>
              <li>Писать хранимые процедуры</li>
              <li>Писать запросы, используя локальные переменные</li>
            </ul>
          </Row>
          <Row className="my-5">
            <p className="text-black fs-2">
              Дополнительная литература
            </p>
            <ul className="ms-5 mt-3 lh-lg">
              <li><a href="https://learn.microsoft.com/ru-ru/sql/t-sql/language-reference?view=sql-server-ver15">Справочник по Transact-SQL | Microsoft Learn</a></li>
              <li><a href="https://info-comp.ru/programmirovanie/412-directory-transact-sql.html">Справочник Transact-SQL – основы для новичков | info-comp.ru</a></li>
              <li>Microsoft SQL Server 2012, основы T-SQL, Бен-Ган И., Райтман М.А., 2015.</li>
              <li><a href="https://metanit.com/sql/sqlserver/">MS SQL Server 2022 и T-SQL | metanit.com</a></li>
            </ul>
          </Row>
          <br />
        </Container>
      </Container>
    </Container>
  );
};