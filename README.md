MongoDB.Repository
============================================================================================================
��ּ����MongoDB�Ĳ�����ʽ��ʵ������Entity Framework�ı�����

============================================================================================================

	//����ʵ������Student
    public class Student : Entity
    {
        [BsonIndex]		//��������
        public string Name { get; set; }
        public int Age { get; set; }
    }    
	//����ʵ������Teacher
	public class Teacher : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
	//����ʵ�����ͣ��̳���RefEntity�����ͽ��м��ϲ���
    public class Grade : RefEntity
    {
        public string Name { get; set; }
    }

	//���������ģ���ע�ᶨ���ʵ������
    public class TestDBContext : MongoDBContext
    {
        public TestDBContext() : base("TestDBContext")		//�������ݿ������ַ���
		{ }

        public override void OnRegisterModel(ITypeRegistration registration)
        {
            registration.RegisterType<Student>().RegisterType<Grade>();
        }
	}
	
	//��Ӧ�ó���������ʼ�������������δ���
	MongoDBRepository.RegisterMongoDBContext(new TestDBContext());		//ע���������
    MongoDBRepository.RegisterMongoIndex();								//ע����������������û��ʹ��BsonIndexAttribute����������Ҫ

	//�����ļ���������Ӧ�������ַ����ڵ�
	<configuration>
		<connectionStrings>
			<add name="TestDBContext" connectionString="mongodb://localhost:27017/TestMongo"/>
		</connectionStrings>
	</configuration>

=================================================================================================================

//��ʵ����
Student student = new Student()
student.Name = "hyf";
student.Age = 30;
student.Save();

//��ʵ���ϱ���
MongoEntity.Save(new List<Student>() {
    new Student{ Name="hyf", Age=33 },
    new Student{ Name="zhc", Age=30 }
});

//ʵ���ѯ
MongoEntity.Get<Student>(student.Id);
MongoEntity.Get<Student>(s => s.Name == "hyf" && s.Age > 33);
MongoEntity.Select<Student>(s => s.Age == 30).ToList();
MongoEntity.Select<Student>(s => s.Age >= 19 && s.Age <= 22, s => s.Age, pageIndex=1, pageSize=2, out pageCount, out allCount).ToList();

//ɾ������
MongoEntity.RemoveAll<Student>(e => e.Name == "hyf");

//ͳ��
MongoEntity.Count<Student>(s => s.Age == 30)

//������������ο�MongoEntity

=================================================================================================================

//���
grade = new Grade();
grade.Name = "No1";
foreach (Student student in students)
    grade.Add<Student>(student);
foreach (Teacher teacher in teachers)
    grade.Add<Teacher>(teacher);
grade.Update();		//��grade�����Ӽ��е�Student��Teacher���������ݣ���������Ϊ������ӣ�������¡�

//��ѯ�Ӽ��е�����
grade.Pick<Student>("BsonId string").Name

//�Ӽ�ͳ��
grade.Count<Student>()	//ͳ���Ӽ�������Student����

//�����Ӽ���������������ο�IRefEntity�ӿ�

=================================================================================================================

Auther:WinHu
Blog:http://www.cnblogs.com/winhu/

��ӭ���ʹ�ò�����޸������