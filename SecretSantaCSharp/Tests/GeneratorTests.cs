using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace SecretSantaCSharp.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void testWhenTwoPeopleAreListedThenTheyAreBothSantasOfEachOther()
        {
            Person person1 = new Person("Adam", "Arlington");
            Person person2 = new Person("Askara", "Barnes");

            Generator testObject = new Generator();

            Person[] people = new Person[] {person1, person2};
            Assignment[] assignments = testObject.createAssignmentsForPeople(people);

            Assert.AreEqual(assignments.Length, 2);
            Assignment assignment1 = assignments[0];
            Assignment assignment2 = assignments[1];
            Assert.AreSame(assignment1.giver, person1);
            Assert.AreSame(assignment1.recipient, person2);
            Assert.AreSame(assignment1.giver, person2);
            Assert.AreSame(assignment1.recipient, person1);
        }

        [Test]
        public void testWhenSeveralPeopleAreMatchedThenEveryoneIsMatched()
        {
            Person person1 = new Person("Adam", "Arlington");
            Person person2 = new Person("Askara", "Barnes");
            Person person3 = new Person("Aaron", "Chung");

            Generator testObject = new Generator();

            Person[] people = new Person[] { person1, person2, person3 };
            Assignment[] assignments = testObject.createAssignmentsForPeople(people);
            Person[] givers = this.giversFromAssignments(assignments);
            Assert.Contains(person1, givers);
            Assert.Contains(person2, givers);
            Assert.Contains(person3, givers);
            Person[] recipients = this.recipientsFromAssignments(assignments);
            Assert.Contains(person1, recipients);
            Assert.Contains(person2, recipients);
            Assert.Contains(person3, recipients);
            Assert.False(this.assignmentsContainSamerPerson(assignments));
        }

        [Test]
        public void testWhenPeopleWithTheSameLastNameAreAddedThenAssignmentsDoNotContainFamilyMembers()
        {
            Person person1 = new Person("Adam", "Arlington");
            Person person2 = new Person("Bob", "Arlington");
            Person person3 = new Person("Askara", "Barnes");
            Person person4 = new Person("Billy", "Barnes");
            Person person5 = new Person("Aaron", "Chung");

            Generator testObject = new Generator();

            Person[] people = new Person[] { person1, person2, person3, person4, person5 };
            Assignment[] assignments = testObject.createAssignmentsForPeople(people);

            Person[] givers = this.giversFromAssignments(assignments);
            Assert.Contains(person1, givers);
            Assert.Contains(person2, givers);
            Assert.Contains(person3, givers);
            Assert.Contains(person4, givers);
            Assert.Contains(person5, givers);
            Person[] recipients = this.recipientsFromAssignments(assignments);
            Assert.Contains(person1, recipients);
            Assert.Contains(person2, recipients);
            Assert.Contains(person3, recipients);
            Assert.Contains(person4, recipients);
            Assert.Contains(person5, recipients);
            Assert.False(this.assignmentsContainSamerPerson(assignments));
            Assert.False(this.assignmentsContainSamerPerson(assignments));
        }

        [Test]
        public void testWhenOnlyOnePersonExistsThenNullIsReturned()
        {
            Person person1 = new Person("Adam", "Arlington");
            Generator testObject = new Generator();

            Person[] people = new Person[] { person1 };
            Assignment[] assignments = testObject.createAssignmentsForPeople(people);

            Assert.Null(assignments);
        }

        [Test]
        public void testWhenNoMatchesExistBecuaseOfTooManyOfOneFamilyThenNullIsReturned()
        {
            Person person1 = new Person("Adam", "Arlington");
            Person person2 = new Person("Bob", "Arlington");
            Person person3 = new Person("Charlie", "Arlington");
            Person person4 = new Person("Alice", "Bones");
            Person person5 = new Person("Betty", "Bones");

            Generator testObject = new Generator();

            Person[] people = new Person[] { person1, person2, person3, person4, person5 };
            Assignment[] assignments = testObject.createAssignmentsForPeople(people);

            Assert.Null(assignments);
        }

        [Test]
        public void testAllMatchesCanBeCreated()
        {
            Person person1 = new Person("Adam", "Arlington");
            Person person2 = new Person("Bob", "Arlington");
            Person person3 = new Person("Askara", "Barnes");
            Person person4 = new Person("Billy", "Barnes");
            Person person5 = new Person("Aaron", "Chung");

            Generator testObject = new Generator();

            Person[] people = new Person[] { person1, person2, person3, person4, person5 };
            Person[] expectedRecipients = new Person[] { person5, person3, person2, person1, person4 };

            bool found = false;
            int count = 0;

            while (count < 20000 && !found)
            {
                count++;
                Assignment[] assignments = testObject.createAssignmentsForPeople(people);
                Person[] recipients = this.recipientsFromAssignments(assignments);
                found = Enumerable.SequenceEqual(recipients, expectedRecipients);
            }

            Assert.Null(found);
        }

        [Test]
        public void testGiversAreReturnedInAlphabeticalOrder()
        {
            Person person1 = new Person("Adam", "Arlington");
            Person person2 = new Person("Bob", "Arlington");
            Person person3 = new Person("Askara", "Barnes");
            Person person4 = new Person("Billy", "Barnes");
            Person person5 = new Person("Aaron", "Chung");

            Generator testObject = new Generator();

            Person[] people = new Person[] { person1, person2, person3, person4, person5 };
            Person[] expectedGivers = new Person[] { person5, person3, person2, person1, person4 };
            Assignment[] assignments = testObject.createAssignmentsForPeople(people);
            Person[] givers = this.giversFromAssignments(assignments);
            Assert.True(Enumerable.SequenceEqual(expectedGivers, givers));
        }

        protected Person[] giversFromAssignments(Assignment[] assignments)
        {
            var givers = new System.Collections.Generic.List<Person>();
            foreach (Assignment assignment in assignments)
            {
                givers.Add(assignment.giver);   
            }
            return givers.ToArray();
        }

        protected Person[] recipientsFromAssignments(Assignment[] assignments)
        {
            var recipients = new System.Collections.Generic.List<Person>();
            foreach (Assignment assignment in assignments)
            {
                recipients.Add(assignment.recipient);
            }
            return recipients.ToArray();
        }

        protected bool assignmentsContainSamerPerson(Assignment[] assignments)
        {
            bool samePerson = false;
            foreach (Assignment assignment in assignments)
            {
                samePerson |= assignment.giver == assignment.recipient;
            }
            return samePerson;
        }

        protected bool assignmentsContainTwoPeopleFromTheSameFamily(Assignment[] assignments)
        {
            bool sameFamily = false;
            foreach (Assignment assignment in assignments)
            {
                sameFamily |= assignment.giver.lastName == assignment.recipient.lastName;
            }
            return sameFamily;
        }
    }
}

 